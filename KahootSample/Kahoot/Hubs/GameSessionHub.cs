using Microsoft.AspNetCore.SignalR;
using Services.IService;
using Services.RequestAndResponse.Enum;
using Services.RequestAndResponse.Request.GameSessionRequest;
using Services.RequestAndResponse.Request.QuestionInGameRequest;
using Services.RequestAndResponse.Request.ResponseRequest;
using Services.RequestAndResponse.Response.GameSessionResponses;
using Services.RequestAndResponse.Response;
using System;
using System.Threading.Tasks;
using System.Linq;
using Services.RequestAndResponse.PlayerRequest;
using Services.RequestAndResponse.Request.PlayerRequest;

namespace Kahoot.Hubs
{
    public class GameSessionHub : Hub
    {
        private readonly IGameSessionService _gameSessionService;
        private readonly IQuestionService _questionService;
        private readonly IResponseService _responseService;
        private readonly IQuestionInGameService _questionInGameService;
        private readonly IPlayerService _playerService;

        public GameSessionHub(
            IGameSessionService gameSessionService,
            IQuestionService questionService,
            IResponseService responseService,
            IQuestionInGameService questionInGameService,
            IPlayerService playerService)
        {
            _gameSessionService = gameSessionService;
            _questionService = questionService;
            _responseService = responseService;
            _questionInGameService = questionInGameService;
            _playerService = playerService;
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

                // Gửi thông báo GameStarted đến tất cả người chơi trong group
                await Clients.Group(sessionId.ToString()).SendAsync("GameStarted", sessionId);

                if (questions.Data.Count() < 2)
                {
                    await Clients.Caller.SendAsync("Error", "Not enough questions in the quiz (at least 2 required)");
                    return;
                }

                var secondQuestion = questions.Data.ElementAt(1);
                await Clients.Group(sessionId.ToString()).SendAsync("ReceiveQuestion", secondQuestion);

                // Gửi câu hỏi đầu tiên (nếu có)
                //var firstQuestion = questions.Data.First();
                //await Clients.Group(sessionId.ToString()).SendAsync("ReceiveQuestion", firstQuestion);
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
                var players = await _playerService.GetPlayersBySessionIdAsync(gameSession.SessionId);
                int playerCount = players.Data?.Count() ?? 0; // Handle null case

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
        // Other methods (CreateGameSession, JoinGameSession, etc.) remain unchanged
        //Response
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
                int score = isCorrect ? 100 : 0; // Simplified scoring logic

                var responseRequest = new CreateResponseRequest
                {
                    PlayerId = playerId,
                    QuestionInGameId = questionInGameId,
                    SelectedOption = selectedOption,
                    ResponseTime = 0, // You might calculate this on the client side
                    Score = score,
                    Streak = 0, // Update streak logic as needed
                    Rank = 0 // Update rank logic as needed
                };

                var responseResult = await _responseService.CreateResponseAsync(responseRequest);
                if (responseResult.StatusCode != StatusCodeEnum.Created_201 || responseResult.Data == null)
                {
                    await Clients.Caller.SendAsync("Error", responseResult.Message);
                    return;
                }

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
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", $"An error occurred: {ex.Message}");
            }
        }
        //Question
        public async Task NextQuestion(int sessionId, int currentOrderIndex)
        {
            try
            {
                var questionsInGame = await _questionInGameService.GetQuestionsInGameBySessionIdAsync(sessionId);
                if (questionsInGame.StatusCode != StatusCodeEnum.OK_200 || questionsInGame.Data == null)
                {
                    await Clients.Caller.SendAsync("Error", "No questions found for this session");
                    return;
                }

                var nextQuestion = questionsInGame.Data.FirstOrDefault(q => q.OrderIndex == currentOrderIndex + 1);
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
                var questionsInGame = await _questionInGameService.GetQuestionsInGameBySessionIdAsync(sessionId);
                if (questionsInGame.StatusCode != StatusCodeEnum.OK_200 || questionsInGame.Data == null)
                {
                    await Clients.Caller.SendAsync("Error", "No questions found for this session");
                    return;
                }

                var previousQuestion = questionsInGame.Data.FirstOrDefault(q => q.OrderIndex == currentOrderIndex - 1);
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

                await Clients.Group(sessionId.ToString()).SendAsync("ReceiveQuestion", question.Data);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", $"An error occurred: {ex.Message}");
            }
        }

        //QuestionInGame


    }
}