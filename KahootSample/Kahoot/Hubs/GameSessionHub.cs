using Microsoft.AspNetCore.SignalR;
using Services.IService;
using Services.RequestAndResponse.Enum;
using Services.RequestAndResponse.Request.GameSessionRequest;
using Services.RequestAndResponse.Request.QuestionInGameRequest;
using Services.RequestAndResponse.Request.ResponseRequest;
using Services.RequestAndResponse.Response.GameSessionResponses;
using Services.RequestAndResponse.Response;
using Services.RequestAndResponse.PlayerRequest;
using Services.RequestAndResponse.Request.PlayerRequest;
using StackExchange.Redis;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Services.RequestAndResponse.Request.TeamResultRequest;
using BOs.Model;

namespace Kahoot.Hubs
{
    public class GameSessionHub : Hub
    {
        private readonly IGameSessionService _gameSessionService;
        private readonly ITeamResultInGameService _teamResultService;
        private readonly IQuestionService _questionService;
        private readonly IResponseService _responseService;
        private readonly IQuestionInGameService _questionInGameService;
        private readonly IPlayerService _playerService;
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _redisDb;

        private readonly Dictionary<int, System.Timers.Timer> _sessionTimers = new Dictionary<int, System.Timers.Timer>();
        public GameSessionHub(
            IGameSessionService gameSessionService,
            ITeamResultInGameService teamResultService,
            IQuestionService questionService,
            IResponseService responseService,
            IQuestionInGameService questionInGameService,
            IPlayerService playerService,
            IConnectionMultiplexer redis)
        {
            _gameSessionService = gameSessionService;
            _teamResultService = teamResultService;
            _questionService = questionService;
            _responseService = responseService;
            _questionInGameService = questionInGameService;
            _playerService = playerService;
            _redis = (ConnectionMultiplexer)redis;

            // Kiểm tra kết nối Redis
            if (!_redis.IsConnected)
            {
                throw new InvalidOperationException("Cannot connect to Redis server.");
            }
            _redisDb = _redis.GetDatabase();
        }

        public async Task StartGameSession(int sessionId)
        {
            try
            {
                if (sessionId <= 0)
                {
                    await Clients.Caller.SendAsync("Error", "Invalid Session ID");
                    return;
                }

                var sessionResponse = await _gameSessionService.GetGameSessionByIdAsync(sessionId);
                if (sessionResponse.StatusCode != StatusCodeEnum.OK_200 || sessionResponse.Data == null)
                {
                    await Clients.Caller.SendAsync("Error", "GameSession not found");
                    return;
                }

                if (sessionResponse.Data.Status == "Started")
                {
                    await Clients.Caller.SendAsync("Error", "GameSession already started");
                    return;
                }

                var updateRequest = new UpdateGameSessionRequest
                {
                    QuizId = sessionResponse.Data.QuizId,
                    StartedAt = sessionResponse.Data.StartedAt ?? DateTime.UtcNow,
                    Status = "Started",
                    Pin = sessionResponse.Data.Pin,
                    EnableSpeedBonus = sessionResponse.Data.EnableSpeedBonus,
                    EnableStreak = sessionResponse.Data.EnableStreak,
                    GameMode = sessionResponse.Data.GameMode,
                    MaxPlayers = sessionResponse.Data.MaxPlayers,
                    AutoAdvance = sessionResponse.Data.AutoAdvance,
                    ShowLeaderboard = sessionResponse.Data.ShowLeaderboard
                };

                var updateResult = await _gameSessionService.UpdateGameSessionAsync(sessionId, updateRequest);
                if (updateResult.StatusCode != StatusCodeEnum.OK_200)
                {
                    await Clients.Caller.SendAsync("Error", updateResult.Message);
                    return;
                }

                // Tạo QuestionInGame tự động khi GameSession bắt đầu
                var questions = await _questionService.GetQuestionsByQuizIdAsync(sessionResponse.Data.QuizId);
                if (questions.StatusCode != StatusCodeEnum.OK_200 || questions.Data == null || !questions.Data.Any())
                {
                    await Clients.Caller.SendAsync("Error", "No questions found for this quiz");
                    return;
                }

                var createRequests = questions.Data.Select((q, index) => new CreateQuestionInGameRequest
                {
                    SessionId = sessionId,
                    QuestionId = q.QuestionId,
                    OrderIndex = index + 1,
                    Status = "Pending"
                }).ToList();

                foreach (var request in createRequests)
                {
                    var result = await _questionInGameService.CreateQuestionInGameAsync(request);
                    if (result.StatusCode != StatusCodeEnum.Created_201)
                    {
                        await Clients.Caller.SendAsync("Error", "Failed to create QuestionInGame");
                        return;
                    }
                }

                // Lưu danh sách QuestionInGame vào Redis
                string questionKey = $"session:{sessionId}:questions";
                foreach (var request in createRequests)
                {
                    var questionJson = JsonSerializer.Serialize(request);
                    await _redisDb.ListRightPushAsync(questionKey, questionJson);
                }
                await _redisDb.KeyExpireAsync(questionKey, TimeSpan.FromHours(24));

                // Lưu trạng thái câu hỏi hiện tại (current question index)
                bool indexSet = await _redisDb.StringSetAsync($"session:{sessionId}:currentQuestionIndex", "0");
                if (!indexSet)
                {
                    await Clients.Caller.SendAsync("Error", "Failed to set current question index in Redis");
                    return;
                }
                await _redisDb.KeyExpireAsync($"session:{sessionId}:currentQuestionIndex", TimeSpan.FromHours(24));

                // Gửi thông báo GameStarted đến tất cả người chơi trong group
                await Clients.Group(sessionId.ToString()).SendAsync("GameStarted", sessionId);

                if (questions.Data.Count() < 2)
                {
                    await Clients.Caller.SendAsync("Error", "Not enough questions in the quiz (at least 2 required)");
                    return;
                }

                var secondQuestion = questions.Data.ElementAt(1);
                await Clients.Group(sessionId.ToString()).SendAsync("ReceiveQuestion", secondQuestion);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", $"An error occurred: {ex.Message}");
            }
        }

        public async Task JoinGameSession(string pin, string nickname)
        {
            try
            {
                if (string.IsNullOrEmpty(pin) || string.IsNullOrEmpty(nickname))
                {
                    await Clients.Caller.SendAsync("Error", "PIN and Nickname are required");
                    return;
                }

                // Find the GameSession by PIN
                var sessionResponse = await _gameSessionService.GetGameSessionByPinAsync(pin);
                if (sessionResponse.StatusCode != StatusCodeEnum.OK_200 || sessionResponse.Data == null)
                {
                    await Clients.Caller.SendAsync("Error", "Invalid PIN or GameSession not found");
                    return;
                }

                var gameSession = sessionResponse.Data;
                if (gameSession.Status != "Pending")
                {
                    await Clients.Caller.SendAsync("Error", "Game has already started or ended");
                    return;
                }

                // Lấy danh sách người chơi từ Redis
                string playersKey = $"session:{gameSession.SessionId}:players";
                var playerCount = (int)await _redisDb.ListLengthAsync(playersKey);

                if (gameSession.MaxPlayers <= 0 || playerCount >= gameSession.MaxPlayers)
                {
                    await Clients.Caller.SendAsync("Error", "Session is full");
                    return;
                }

                // Create the Player using PlayerService
                var createPlayerRequest = new CreatePlayerRequest
                {
                    SessionId = gameSession.SessionId,
                    Nickname = nickname,
                    JoinedAt = DateTime.UtcNow,
                    Score = 0,
                    Ranking = 0
                };

                var playerResponse = await _playerService.AddPlayerAsync(createPlayerRequest);
                if (playerResponse.StatusCode != StatusCodeEnum.Created_201 || playerResponse.Data == null)
                {
                    await Clients.Caller.SendAsync("Error", playerResponse.Message ?? "Failed to add player");
                    return;
                }

                // Lưu thông tin người chơi vào Redis
                var playerJson = JsonSerializer.Serialize(playerResponse.Data);
                await _redisDb.ListRightPushAsync(playersKey, playerJson);
                await _redisDb.KeyExpireAsync(playersKey, TimeSpan.FromHours(24));

                // Add the player to the SignalR group for this session
                await Groups.AddToGroupAsync(Context.ConnectionId, gameSession.SessionId.ToString());

                // Notify the caller (the player) that they joined successfully
                await Clients.Caller.SendAsync("JoinedGame", new { SessionId = gameSession.SessionId, PlayerId = playerResponse.Data.PlayerId, Nickname = nickname });

                // Notify all other clients in the group (e.g., host and other players) that a new player joined
                await Clients.Group(gameSession.SessionId.ToString()).SendAsync("PlayerJoined", new { PlayerId = playerResponse.Data.PlayerId, Nickname = nickname, JoinedAt = createPlayerRequest.JoinedAt });
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", $"An error occurred: {ex.Message}");
            }
        }

        public async Task SubmitResponse(int playerId, int questionInGameId, string selectedOptions, float responseTime)
        {
            try
            {
                // Lấy thông tin QuestionInGame
                var questionInGame = await _questionInGameService.GetQuestionInGameByIdAsync(questionInGameId);
                if (questionInGame.StatusCode != StatusCodeEnum.OK_200 || questionInGame.Data == null)
                {
                    await Clients.Caller.SendAsync("Error", "QuestionInGame not found");
                    return;
                }

                // Lấy thông tin Question
                var question = await _questionService.GetQuestionByIdAsync(questionInGame.Data.QuestionId);
                if (question.StatusCode != StatusCodeEnum.OK_200 || question.Data == null)
                {
                    await Clients.Caller.SendAsync("Error", "Question not found");
                    return;
                }

                // Lấy thông tin GameSession để kiểm tra cấu hình
                var session = await _gameSessionService.GetGameSessionByIdAsync(questionInGame.Data.SessionId);
                if (session.StatusCode != StatusCodeEnum.OK_200 || session.Data == null)
                {
                    await Clients.Caller.SendAsync("Error", "GameSession not found");
                    return;
                }

                // Kiểm tra và phân tích đáp án (hỗ trợ multiple-choice với partial scoring)
                bool isCorrect = false;
                float correctnessRatio = 0;
                if (!string.IsNullOrEmpty(question.Data.CorrectOptions) && !string.IsNullOrEmpty(selectedOptions))
                {
                    try
                    {
                        var correctOptionSet = question.Data.CorrectOptions.Split(',').Select(int.Parse).ToHashSet();
                        var selectedOptionSet = selectedOptions.Split(',').Select(int.Parse).ToHashSet();
                        isCorrect = selectedOptionSet.SetEquals(correctOptionSet); // Exact match
                        if (!isCorrect && correctOptionSet.Count > 0)
                        {
                            var correctSelections = selectedOptionSet.Count(option => correctOptionSet.Contains(option));
                            correctnessRatio = (float)correctSelections / correctOptionSet.Count; // Partial correctness
                        }
                    }
                    catch (Exception)
                    {
                        await Clients.Caller.SendAsync("Error", "Invalid selected options format");
                        return;
                    }
                }

                // Tính điểm cơ bản với partial scoring
                int baseScore = isCorrect ? 100 : (int)(100 * correctnessRatio);
                float speedBonus = 0;
                int streakBonus = 0;
                int streak = 0;

                // Tính điểm thưởng tốc độ (SpeedBonus) nếu EnableSpeedBonus = true
                if (baseScore > 0 && session.Data.EnableSpeedBonus && question.Data != null && question.Data.TimeLimit != null)
                {
                    float timeLimit = ((int?)question.Data.TimeLimit).Value;
                    if (responseTime >= 0 && responseTime <= timeLimit)
                    {
                        float remainingTime = timeLimit - responseTime;
                        speedBonus = baseScore * (remainingTime / timeLimit); // Tỷ lệ thời gian còn lại
                    }
                }

                // Tính điểm thưởng streak nếu EnableStreak = true (chỉ tăng nếu đáp án hoàn toàn đúng)
                if (isCorrect && session.Data.EnableStreak)
                {
                    // Lấy lịch sử phản hồi của người chơi để tính streak
                    var previousResponses = await _responseService.GetResponsesByPlayerIdAsync(playerId);
                    if (previousResponses.StatusCode == StatusCodeEnum.OK_200 && previousResponses.Data != null)
                    {
                        var lastResponse = previousResponses.Data
                            .OrderByDescending(r => r.ResponseId)
                            .FirstOrDefault();
                        streak = lastResponse != null && lastResponse.Score == 100 ? lastResponse.Streak + 1 : 1; // Chỉ tăng streak nếu score đầy đủ
                    }
                    else
                    {
                        streak = 1; // Câu trả lời đúng đầu tiên
                    }
                    streakBonus = streak * 10; // Ví dụ: 10 điểm cho mỗi câu trong streak
                }
                else
                {
                    streak = 0; // Reset streak nếu không hoàn toàn đúng
                }

                // Tổng điểm
                int totalScore = baseScore + (int)speedBonus + streakBonus;

                // Tạo Response
                var responseRequest = new CreateResponseRequest
                {
                    PlayerId = playerId,
                    QuestionInGameId = questionInGameId,
                    SelectedOptions = selectedOptions,
                    ResponseTime = (int)responseTime,
                    Score = totalScore,
                    Streak = streak,
                    Rank = 0 // Sẽ cập nhật sau khi tính bảng xếp hạng
                };
                Console.WriteLine($"Submitting response: PlayerId={playerId}, QuestionInGameId={questionInGameId}, SelectedOptions={selectedOptions}, Score={totalScore}, Streak={streak}");

                // Lưu Response vào database
                var responseResult = await _responseService.CreateResponseAsync(responseRequest);
                if (responseResult.StatusCode != StatusCodeEnum.Created_201 || responseResult.Data == null)
                {
                    Console.WriteLine($"Failed to save Response to database: {responseResult.Message}");
                    await Clients.Caller.SendAsync("Error", responseResult.Message);
                    return;
                }

                // Lưu Response vào Redis
                long newResponseCount = 0;
                try
                {
                    string responseKey = $"session:{questionInGame.Data.SessionId}:responses";
                    var responseJson = JsonSerializer.Serialize(responseRequest);
                    await _redisDb.ListRightPushAsync(responseKey, responseJson);
                    await _redisDb.KeyExpireAsync(responseKey, TimeSpan.FromHours(24));

                    string responseCountKey = $"session:{questionInGame.Data.SessionId}:responseCount";
                    newResponseCount = await _redisDb.StringIncrementAsync(responseCountKey);
                    await _redisDb.KeyExpireAsync(responseCountKey, TimeSpan.FromHours(24));
                }
                catch (Exception redisEx)
                {
                    Console.WriteLine($"Error while saving to Redis: {redisEx.Message}");
                }

                // Cập nhật điểm của Player
                var player = await _playerService.GetPlayerByIdAsync(playerId);
                if (player.StatusCode != StatusCodeEnum.OK_200 || player.Data == null)
                {
                    await Clients.Caller.SendAsync("Error", "Player not found");
                    return;
                }

                var updatePlayerRequest = new UpdatePlayerRequest
                {
                    PlayerId = playerId,
                    SessionId = player.Data.SessionId,
                    Score = (player.Data.Score ?? 0) + totalScore
                };
                var updatePlayerResult = await _playerService.UpdatePlayerAsync(updatePlayerRequest);
                if (updatePlayerResult.StatusCode != StatusCodeEnum.OK_200)
                {
                    await Clients.Caller.SendAsync("Error", updatePlayerResult.Message);
                    return;
                }

                // Cập nhật điểm của Team (nếu có)
                if (player.Data.TeamId.HasValue && player.Data.TeamId > 0)
                {
                    var teamId = player.Data.TeamId.Value;

                    var existingTeamResults = await _teamResultService.GetTeamResultsBySessionIdAsync(player.Data.SessionId);
                    var existingResult = existingTeamResults.Data?
                        .FirstOrDefault(tr => tr.TeamId == teamId && tr.QuestionInGameId == questionInGameId);

                    if (existingResult != null)
                    {
                        var updateRequest = new UpdateTeamResultRequest
                        {
                            TeamResultInGameId = existingResult.TeamResultInGameId,
                            SessionId = player.Data.SessionId,
                            QuestionInGameId = questionInGameId,
                            TeamId = teamId,
                            Score = existingResult.Score + totalScore
                        };
                        await _teamResultService.UpdateTeamResultAsync(updateRequest);
                    }
                    else
                    {
                        var createRequest = new CreateTeamResultRequest
                        {
                            SessionId = player.Data.SessionId,
                            QuestionInGameId = questionInGameId,
                            TeamId = teamId,
                            Score = totalScore
                        };
                        await _teamResultService.CreateTeamResultAsync(createRequest);
                    }

                    var leaderboardResult = await _teamResultService.GetTeamRankingsBySessionIdAsync(player.Data.SessionId);
                    if (leaderboardResult.StatusCode == StatusCodeEnum.OK_200 && leaderboardResult.Data != null)
                    {
                        await Clients.Group(player.Data.SessionId.ToString()).SendAsync("TeamLeaderboardUpdated", leaderboardResult.Data);
                    }
                }

                // Thông báo kết quả phản hồi cho người chơi
                await Clients.Caller.SendAsync("ResponseSubmitted", new
                {
                    IsCorrect = isCorrect,
                    Score = totalScore,
                    SpeedBonus = (int)speedBonus,
                    StreakBonus = streakBonus,
                    Streak = streak,
                    TotalScore = updatePlayerResult.Data.Score
                });

                // Thông báo cho host về phản hồi mới
                await Clients.Group(player.Data.SessionId.ToString()).SendAsync("PlayerResponded", new
                {
                    PlayerId = playerId,
                    QuestionInGameId = questionInGameId,
                    SelectedOptions = selectedOptions
                });

                // Thông báo số lượng phản hồi đã cập nhật
                await Clients.Group(player.Data.SessionId.ToString()).SendAsync("ResponseCountUpdated", new
                {
                    SessionId = player.Data.SessionId,
                    ResponseCount = newResponseCount
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SubmitResponse error: {ex.Message}\nStackTrace: {ex.StackTrace}");
                await Clients.Caller.SendAsync("Error", $"An error occurred: {ex.Message}");
            }
        }

        public async Task GetResponseCount(int sessionId)
        {
            try
            {
                string responseCountKey = $"session:{sessionId}:responseCount";
                var responseCount = await _redisDb.StringGetAsync(responseCountKey);

                if (!responseCount.HasValue)
                {
                    await Clients.Caller.SendAsync("ResponseCountUpdated", new
                    {
                        SessionId = sessionId,
                        ResponseCount = 0
                    });
                    return;
                }

                await Clients.Caller.SendAsync("ResponseCountUpdated", new
                {
                    SessionId = sessionId,
                    ResponseCount = (long)responseCount
                });
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", $"An error occurred: {ex.Message}");
            }
        }

        public async Task NextQuestion(int sessionId, int questionInGameId)
        {
            try
            {
                if (sessionId <= 0 || questionInGameId <= 0)
                {
                    await Clients.Caller.SendAsync("Error", "Invalid SessionId or QuestionInGameId");
                    return;
                }

                var sessionResponse = await _gameSessionService.GetGameSessionByIdAsync(sessionId);
                if (sessionResponse.StatusCode != StatusCodeEnum.OK_200 || sessionResponse.Data == null)
                {
                    await Clients.Caller.SendAsync("Error", "GameSession not found");
                    return;
                }

                if (sessionResponse.Data.Status != "Started")
                {
                    await Clients.Caller.SendAsync("Error", "GameSession is not in progress");
                    return;
                }

                var currentQuestionInGame = await _questionInGameService.GetQuestionInGameByIdAsync(questionInGameId);
                if (currentQuestionInGame.StatusCode != StatusCodeEnum.OK_200 || currentQuestionInGame.Data == null)
                {
                    await Clients.Caller.SendAsync("Error", "Current QuestionInGame not found");
                    return;
                }

                if (currentQuestionInGame.Data.SessionId != sessionId)
                {
                    await Clients.Caller.SendAsync("Error", "QuestionInGame does not belong to this session");
                    return;
                }

                int currentOrderIndex = currentQuestionInGame.Data.OrderIndex;

                string questionKey = $"session:{sessionId}:questions";
                var questionsInGame = new List<CreateQuestionInGameRequest>();
                var questionValues = await _redisDb.ListRangeAsync(questionKey);
                if (questionValues == null || !questionValues.Any())
                {
                    await Clients.Caller.SendAsync("Error", "No questions found for this session in Redis");
                    return;
                }

                foreach (var value in questionValues)
                {
                    try
                    {
                        var deserializedQuestion = JsonSerializer.Deserialize<CreateQuestionInGameRequest>(value);
                        if (deserializedQuestion != null)
                        {
                            questionsInGame.Add(deserializedQuestion);
                        }
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Failed to deserialize question: {ex.Message}");
                        continue;
                    }
                }

                if (!questionsInGame.Any())
                {
                    await Clients.Caller.SendAsync("Error", "No valid questions found for this session");
                    return;
                }

                var nextQuestionInGame = questionsInGame.FirstOrDefault(q => q.OrderIndex == currentOrderIndex + 1);
                if (nextQuestionInGame == null)
                {
                    await EndGameSession(sessionId);
                    return;
                }

                var question = await _questionService.GetQuestionByIdAsync(nextQuestionInGame.QuestionId);
                if (question.StatusCode != StatusCodeEnum.OK_200 || question.Data == null)
                {
                    await Clients.Caller.SendAsync("Error", "Next question not found");
                    return;
                }

                var nextQuestionInGameEntity = await _questionInGameService.GetQuestionInGameBySessionIdAndQuestionIdAsync(sessionId, nextQuestionInGame.QuestionId);
                if (nextQuestionInGameEntity.StatusCode != StatusCodeEnum.OK_200 || nextQuestionInGameEntity.Data == null)
                {
                    await Clients.Caller.SendAsync("Error", "Next QuestionInGame not found in database");
                    return;
                }
                int nextQuestionInGameId = nextQuestionInGameEntity.Data.QuestionInGameId;

                string indexKey = $"session:{sessionId}:currentQuestionIndex";
                bool indexSet = await _redisDb.StringSetAsync(indexKey, nextQuestionInGame.OrderIndex.ToString());
                if (!indexSet)
                {
                    await Clients.Caller.SendAsync("Error", "Failed to update current question index in Redis");
                    return;
                }
                await _redisDb.KeyExpireAsync(indexKey, TimeSpan.FromHours(24));

                string responseCountKey = $"session:{sessionId}:responseCount";
                await _redisDb.KeyDeleteAsync(responseCountKey);

                // Gửi câu hỏi tiếp theo
                await Clients.Group(sessionId.ToString()).SendAsync("ReceiveQuestion", new
                {
                    QuestionInGameId = nextQuestionInGameId,
                    Question = question.Data
                });

                // Thêm timer cho câu hỏi hiện tại
                StartQuestionTimer(sessionId, nextQuestionInGameId, question.Data.TimeLimit);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"NextQuestion error: {ex.Message}\nStackTrace: {ex.StackTrace}");
                await Clients.Caller.SendAsync("Error", $"An error occurred: {ex.Message}");
            }
        }
        private void StartQuestionTimer(int sessionId, int questionInGameId, int timeLimitInSeconds)
        {
            // Dừng timer cũ nếu có
            if (_sessionTimers.ContainsKey(sessionId))
            {
                _sessionTimers[sessionId].Stop();
                _sessionTimers[sessionId].Dispose();
                _sessionTimers.Remove(sessionId);
            }

            var timer = new System.Timers.Timer(timeLimitInSeconds * 1000); // Chuyển sang milliseconds
            timer.Elapsed += async (sender, e) => await HandleTimeout(sessionId, questionInGameId);
            timer.AutoReset = false; // Chỉ chạy một lần
            timer.Start();

            _sessionTimers[sessionId] = timer;
        }
        private async Task HandleTimeout(int sessionId, int questionInGameId)
        {
            try
            {
                // Lấy danh sách người chơi trong session từ Redis
                string playersKey = $"session:{sessionId}:players";
                var playerValues = await _redisDb.ListRangeAsync(playersKey);
                var players = new List<Player>();
                foreach (var value in playerValues)
                {
                    try
                    {
                        var player = JsonSerializer.Deserialize<Player>(value);
                        if (player != null)
                        {
                            players.Add(player);
                        }
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Failed to deserialize player: {ex.Message}");
                        continue;
                    }
                }

                // Kiểm tra xem mỗi người chơi đã trả lời chưa
                string responseKey = $"session:{sessionId}:responses";
                var responseValues = await _redisDb.ListRangeAsync(responseKey);
                var respondedPlayerIds = responseValues
                    .Select(r => JsonSerializer.Deserialize<CreateResponseRequest>(r)?.PlayerId)
                    .Where(pid => pid.HasValue)
                    .Select(pid => pid.Value)
                    .ToList();

                foreach (var player in players)
                {
                    if (!respondedPlayerIds.Contains(player.PlayerId))
                    {
                        // Người chơi chưa trả lời, tự động submit với selectedOption = 0
                        await SubmitResponseForTimeout(player.PlayerId, questionInGameId, "", sessionId);
                    }
                }

                // Dừng timer sau khi xử lý
                if (_sessionTimers.ContainsKey(sessionId))
                {
                    _sessionTimers[sessionId].Stop();
                    _sessionTimers[sessionId].Dispose();
                    _sessionTimers.Remove(sessionId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HandleTimeout error: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }
        }
        private async Task SubmitResponseForTimeout(int playerId, int questionInGameId, string selectedOption, int sessionId)
        {
            try
            {
                var questionInGame = await _questionInGameService.GetQuestionInGameByIdAsync(questionInGameId);
                if (questionInGame.StatusCode != StatusCodeEnum.OK_200 || questionInGame.Data == null)
                {
                    return;
                }

                var question = await _questionService.GetQuestionByIdAsync(questionInGame.Data.QuestionId);
                if (question.StatusCode != StatusCodeEnum.OK_200 || question.Data == null)
                {
                    return;
                }

                bool isCorrect = selectedOption == question.Data.CorrectOptions;
                int score = isCorrect ? 100 : 0; // Gán 0 điểm nếu không trả lời (selectedOption = 0)

                var responseRequest = new CreateResponseRequest
                {
                    PlayerId = playerId,
                    QuestionInGameId = questionInGameId,
                    SelectedOptions = selectedOption,
                    ResponseTime = 0,
                    Score = score,
                    Streak = 0,
                    Rank = 0
                };

                var responseResult = await _responseService.CreateResponseAsync(responseRequest);
                if (responseResult.StatusCode == StatusCodeEnum.Created_201 && responseResult.Data != null)
                {
                    string responseKey = $"session:{sessionId}:responses";
                    var responseJson = JsonSerializer.Serialize(responseRequest);
                    await _redisDb.ListRightPushAsync(responseKey, responseJson);
                    await _redisDb.KeyExpireAsync(responseKey, TimeSpan.FromHours(24));

                    string responseCountKey = $"session:{sessionId}:responseCount";
                    long newResponseCount = await _redisDb.StringIncrementAsync(responseCountKey);
                    await _redisDb.KeyExpireAsync(responseCountKey, TimeSpan.FromHours(24));

                    var player = await _playerService.GetPlayerByIdAsync(playerId);
                    if (player.StatusCode == StatusCodeEnum.OK_200 && player.Data != null)
                    {
                        var updatePlayerRequest = new UpdatePlayerRequest
                        {
                            PlayerId = playerId,
                            SessionId = player.Data.SessionId,
                            Score = (player.Data.Score ?? 0) + score
                        };
                        await _playerService.UpdatePlayerAsync(updatePlayerRequest);

                        await Clients.Client(GetConnectionIdByPlayerId(playerId)).SendAsync("ResponseSubmitted", new
                        {
                            IsCorrect = isCorrect,
                            Score = score,
                            TotalScore = updatePlayerRequest.Score
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SubmitResponseForTimeout error: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }
        }

        private string GetConnectionIdByPlayerId(int playerId)
        {
            // Logic để ánh xạ PlayerId với ConnectionId (cần triển khai tùy thuộc vào cách bạn quản lý connection)
            // Đây là ví dụ giả định, bạn cần triển khai thực tế
            return Context.ConnectionId; // Placeholder
        }

        public async Task PreviousQuestion(int sessionId, int currentOrderIndex)
        {
            try
            {
                string indexKey = $"session:{sessionId}:currentQuestionIndex";
                var transaction = _redisDb.CreateTransaction();

                // Kiểm tra và cập nhật currentQuestionIndex trong transaction
                var currentIndexTask = transaction.StringGetAsync(indexKey);
                await transaction.ExecuteAsync();

                if (!currentIndexTask.Result.HasValue)
                {
                    await Clients.Caller.SendAsync("Error", "Current question index not found in Redis");
                    return;
                }

                // Lấy danh sách câu hỏi từ Redis
                string questionKey = $"session:{sessionId}:questions";
                var questionsInGame = new List<CreateQuestionInGameRequest>();
                var questionValues = await _redisDb.ListRangeAsync(questionKey);
                foreach (var value in questionValues)
                {
                    try
                    {
                        var deserializedQuestion = JsonSerializer.Deserialize<CreateQuestionInGameRequest>(value);
                        questionsInGame.Add(deserializedQuestion);
                    }
                    catch (JsonException ex)
                    {
                        await Clients.Caller.SendAsync("Error", $"Failed to deserialize question: {ex.Message}");
                        return;
                    }
                }

                if (!questionsInGame.Any())
                {
                    await Clients.Caller.SendAsync("Error", "No questions found for this session");
                    return;
                }

                var previousQuestion = questionsInGame.FirstOrDefault(q => q.OrderIndex == currentOrderIndex - 1);
                if (previousQuestion == null)
                {
                    await Clients.Caller.SendAsync("Error", "No previous question available");
                    return;
                }

                var question = await _questionService.GetQuestionByIdAsync(previousQuestion.QuestionId);
                if (question.StatusCode != StatusCodeEnum.OK_200 || question.Data == null)
                {
                    await Clients.Caller.SendAsync("Error", "Question not found");
                    return;
                }

                // Cập nhật currentQuestionIndex trong transaction
                transaction.StringSetAsync(indexKey, (currentOrderIndex - 1).ToString());
                bool committed = await transaction.ExecuteAsync();
                if (!committed)
                {
                    await Clients.Caller.SendAsync("Error", "Failed to update current question index in Redis due to transaction failure");
                    return;
                }

                // Reset response count khi quay lại câu hỏi trước
                string responseCountKey = $"session:{sessionId}:responseCount";
                await _redisDb.KeyDeleteAsync(responseCountKey);

                await Clients.Group(sessionId.ToString()).SendAsync("ReceiveQuestion", question.Data);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", $"An error occurred: {ex.Message}");
            }
        }
        //public async Task UpdateQuestionInGameStatus(int questionInGameId, string status)
        //{
        //    try
        //    {
        //        var updateRequest = new UpdateQuestionInGameRequest
        //        {
        //            QuestionInGameId = questionInGameId,
        //            Status = status
        //        };
        //        var updateResult = await _questionInGameService.UpdateQuestionInGameAsync(updateRequest);
        //        if (updateResult.StatusCode != StatusCodeEnum.OK_200 || updateResult.Data == null)
        //        {
        //            await Clients.Caller.SendAsync("Error", updateResult.Message);
        //            return;
        //        }

        //        var sessionId = updateResult.Data.SessionId;
        //        await Clients.Group(sessionId.ToString()).SendAsync("QuestionInGameStatusUpdated", new
        //        {
        //            QuestionInGameId = questionInGameId,
        //            Status = status
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        await Clients.Caller.SendAsync("Error", $"An error occurred: {ex.Message}");
        //    }
        //}
        public async Task ViewSummary(int sessionId)
        {
            try
            {
                var summary = await _gameSessionService.GetSessionSummaryAsync(sessionId);
                if (summary.StatusCode != StatusCodeEnum.OK_200 || summary.Data == null)
                {
                    await Clients.Caller.SendAsync("Error", summary.Message);
                    return;
                }

                await Clients.Group(sessionId.ToString()).SendAsync("ViewSummary", summary.Data);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", $"An error occurred: {ex.Message}");
            }
        }
        public async Task EndGameSession(int sessionId)
        {
            try
            {
                // Xóa dữ liệu Redis liên quan đến phiên chơi
                await _redisDb.KeyDeleteAsync($"session:{sessionId}:questions");
                await _redisDb.KeyDeleteAsync($"session:{sessionId}:responses");
                await _redisDb.KeyDeleteAsync($"session:{sessionId}:players");
                await _redisDb.KeyDeleteAsync($"session:{sessionId}:currentQuestionIndex");
                await _redisDb.KeyDeleteAsync($"session:{sessionId}:responseCount");

                // Cập nhật trạng thái GameSession
                var sessionResponse = await _gameSessionService.GetGameSessionByIdAsync(sessionId);
                if (sessionResponse.StatusCode != StatusCodeEnum.OK_200 || sessionResponse.Data == null)
                {
                    await Clients.Caller.SendAsync("Error", "GameSession not found");
                    return;
                }

                var updateRequest = new UpdateGameSessionRequest
                {
                    QuizId = sessionResponse.Data.QuizId,
                    StartedAt = sessionResponse.Data.StartedAt,
                    Status = "Ended",
                    Pin = sessionResponse.Data.Pin,
                    EnableSpeedBonus = sessionResponse.Data.EnableSpeedBonus,
                    EnableStreak = sessionResponse.Data.EnableStreak,
                    GameMode = sessionResponse.Data.GameMode,
                    MaxPlayers = sessionResponse.Data.MaxPlayers,
                    AutoAdvance = sessionResponse.Data.AutoAdvance,
                    ShowLeaderboard = sessionResponse.Data.ShowLeaderboard
                };
                var updateResult = await _gameSessionService.UpdateGameSessionAsync(sessionId, updateRequest);
                if (updateResult.StatusCode != StatusCodeEnum.OK_200)
                {
                    await Clients.Caller.SendAsync("Error", updateResult.Message);
                    return;
                }

                // Thông báo kết thúc phiên chơi
                await Clients.Group(sessionId.ToString()).SendAsync("GameEnded", sessionId);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", $"An error occurred: {ex.Message}");
            }
        }
    }
}