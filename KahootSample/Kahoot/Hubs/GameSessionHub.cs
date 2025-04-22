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

namespace Kahoot.Hubs
{
    public class GameSessionHub : Hub
    {
        private readonly IGameSessionService _gameSessionService;
        private readonly IQuestionService _questionService;
        private readonly IResponseService _responseService;
        private readonly IQuestionInGameService _questionInGameService;

        public GameSessionHub(
            IGameSessionService gameSessionService,
            IQuestionService questionService,
            IResponseService responseService,
            IQuestionInGameService questionInGameService)
        {
            _gameSessionService = gameSessionService;
            _questionService = questionService;
            _responseService = responseService;
            _questionInGameService = questionInGameService;
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

                // Gửi câu hỏi đầu tiên (nếu có)
                var firstQuestion = questions.Data.First();
                await Clients.Group(sessionId.ToString()).SendAsync("ReceiveQuestion", firstQuestion);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", $"An error occurred: {ex.Message}");
            }
        }

        // Other methods (CreateGameSession, JoinGameSession, etc.) remain unchanged

    }
}