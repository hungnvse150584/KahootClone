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

namespace Kahoot.Hubs
{
    public class GameSessionHub : Hub
    {
        private readonly IGameSessionService _gameSessionService;
        private readonly IQuestionService _questionService;
        private readonly IResponseService _responseService;
        private readonly IQuestionInGameService _questionInGameService;
        private readonly IPlayerService _playerService;
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _redisDb;

        public GameSessionHub(
            IGameSessionService gameSessionService,
            IQuestionService questionService,
            IResponseService responseService,
            IQuestionInGameService questionInGameService,
            IPlayerService playerService,
            IConnectionMultiplexer redis)
        {
            _gameSessionService = gameSessionService;
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

        public async Task SubmitResponse(int playerId, int questionInGameId, int selectedOption)
        {
            try
            {
                var questionInGame = await _questionInGameService.GetQuestionInGameByIdAsync(questionInGameId);
                if (questionInGame.StatusCode != StatusCodeEnum.OK_200 || questionInGame.Data == null)
                {
                    await Clients.Caller.SendAsync("Error", "QuestionInGame not found");
                    return;
                }

                var question = await _questionService.GetQuestionByIdAsync(questionInGame.Data.QuestionId);
                if (question.StatusCode != StatusCodeEnum.OK_200 || question.Data == null)
                {
                    await Clients.Caller.SendAsync("Error", "Question not found");
                    return;
                }

                // Calculate score and correctness
                bool isCorrect = selectedOption == question.Data.CorrectOption;
                int score = isCorrect ? 100 : 0;

                var responseRequest = new CreateResponseRequest
                {
                    PlayerId = playerId,
                    QuestionInGameId = questionInGameId,
                    SelectedOption = selectedOption,
                    ResponseTime = 0,
                    Score = score,
                    Streak = 0,
                    Rank = 0
                };

                var responseResult = await _responseService.CreateResponseAsync(responseRequest);
                if (responseResult.StatusCode != StatusCodeEnum.Created_201 || responseResult.Data == null)
                {
                    await Clients.Caller.SendAsync("Error", responseResult.Message);
                    return;
                }

                // Lưu câu trả lời vào Redis
                string responseKey = $"session:{questionInGame.Data.SessionId}:responses";
                var responseJson = JsonSerializer.Serialize(responseRequest);
                await _redisDb.ListRightPushAsync(responseKey, responseJson);
                await _redisDb.KeyExpireAsync(responseKey, TimeSpan.FromHours(24));

               
                string responseCountKey = $"session:{questionInGame.Data.SessionId}:responseCount";
                long newResponseCount = await _redisDb.StringIncrementAsync(responseCountKey);
                await _redisDb.KeyExpireAsync(responseCountKey, TimeSpan.FromHours(24));

                // Update player's score
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
                    Score = (player.Data.Score ?? 0) + score
                };
                var updatePlayerResult = await _playerService.UpdatePlayerAsync(updatePlayerRequest);
                if (updatePlayerResult.StatusCode != StatusCodeEnum.OK_200)
                {
                    await Clients.Caller.SendAsync("Error", updatePlayerResult.Message);
                    return;
                }

                // Notify the player of their response result
                await Clients.Caller.SendAsync("ResponseSubmitted", new
                {
                    IsCorrect = isCorrect,
                    Score = score,
                    TotalScore = updatePlayerResult.Data.Score
                });

                // Notify the host of the new response
                await Clients.Group(player.Data.SessionId.ToString()).SendAsync("PlayerResponded", new
                {
                    PlayerId = playerId,
                    QuestionInGameId = questionInGameId,
                    SelectedOption = selectedOption
                });

                // Notify all clients in the group about the updated response count
                await Clients.Group(player.Data.SessionId.ToString()).SendAsync("ResponseCountUpdated", new
                {
                    SessionId = player.Data.SessionId,
                    ResponseCount = newResponseCount
                });
            }
            catch (Exception ex)
            {
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

        public async Task NextQuestion(int sessionId, int currentOrderIndex)
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

                var nextQuestion = questionsInGame.FirstOrDefault(q => q.OrderIndex == currentOrderIndex + 1);
                if (nextQuestion == null)
                {
                    await Clients.Caller.SendAsync("Error", "No more questions available");
                    return;
                }

                var question = await _questionService.GetQuestionByIdAsync(nextQuestion.QuestionId);
                if (question.StatusCode != StatusCodeEnum.OK_200 || question.Data == null)
                {
                    await Clients.Caller.SendAsync("Error", "Question not found");
                    return;
                }

                // Cập nhật currentQuestionIndex trong transaction
                transaction.StringSetAsync(indexKey, (currentOrderIndex + 1).ToString());
                bool committed = await transaction.ExecuteAsync();
                if (!committed)
                {
                    await Clients.Caller.SendAsync("Error", "Failed to update current question index in Redis due to transaction failure");
                    return;
                }

                // Reset response count khi chuyển sang câu hỏi mới
                string responseCountKey = $"session:{sessionId}:responseCount";
                await _redisDb.KeyDeleteAsync(responseCountKey);

                await Clients.Group(sessionId.ToString()).SendAsync("ReceiveQuestion", question.Data);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", $"An error occurred: {ex.Message}");
            }
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