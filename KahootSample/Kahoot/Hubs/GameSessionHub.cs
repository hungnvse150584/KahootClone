using Microsoft.AspNetCore.SignalR;
using Services.IService;
using Services.RequestAndResponse.Enum;
using Services.RequestAndResponse.Request.ResponseRequest;
using Services.RequestAndResponse.Response.GameSessionResponses;
using System;
using System.Threading.Tasks;

namespace Kahoot.Hubs
{
    public class GameSessionHub : Hub
    {
        private readonly IGameSessionService _gameSessionService;
        private readonly IQuestionService _questionService;
        private readonly IResponseService _responseService;

        public GameSessionHub(
            IGameSessionService gameSessionService,
            IQuestionService questionService,
            IResponseService responseService)
        {
            _gameSessionService = gameSessionService;
            _questionService = questionService;
            _responseService = responseService;
        }

        public async Task JoinGameSession(int sessionId, int playerId)
        {
            try
            {
                if (sessionId <= 0 || playerId <= 0)
                {
                    await Clients.Caller.SendAsync("Error", "Invalid Session ID or Player ID");
                    return;
                }

                var sessionResponse = await _gameSessionService.GetGameSessionByIdAsync(sessionId);
                if (sessionResponse.StatusCode != StatusCodeEnum.OK_200 || sessionResponse.Data == null)
                {
                    await Clients.Caller.SendAsync("Error", "GameSession not found");
                    return;
                }

                // Lưu sessionId vào Context.Items
                Context.Items["SessionId"] = sessionId.ToString();

                // Thêm người chơi vào group của phiên chơi
                await Groups.AddToGroupAsync(Context.ConnectionId, sessionId.ToString());
                await Clients.Group(sessionId.ToString()).SendAsync("PlayerJoined", playerId);

                var players = await _gameSessionService.GetPlayersInSessionAsync(sessionId);
                if (players.StatusCode == StatusCodeEnum.OK_200 && players.Data != null)
                {
                    await Clients.Group(sessionId.ToString()).SendAsync("PlayerListUpdated", players.Data);
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", $"An error occurred: {ex.Message}");
            }
        }

        public async Task SendQuestion(int sessionId, int questionId)
        {
            try
            {
                if (sessionId <= 0 || questionId <= 0)
                {
                    await Clients.Group(sessionId.ToString()).SendAsync("Error", "Invalid Session ID or Question ID");
                    return;
                }

                var sessionResponse = await _gameSessionService.GetGameSessionByIdAsync(sessionId);
                if (sessionResponse.StatusCode != StatusCodeEnum.OK_200 || sessionResponse.Data == null)
                {
                    await Clients.Group(sessionId.ToString()).SendAsync("Error", "GameSession not found");
                    return;
                }

                var question = await _questionService.GetQuestionByIdAsync(questionId);
                if (question.StatusCode != StatusCodeEnum.OK_200 || question.Data == null)
                {
                    await Clients.Group(sessionId.ToString()).SendAsync("Error", "Question not found");
                    return;
                }

                await Clients.Group(sessionId.ToString()).SendAsync("ReceiveQuestion", question.Data);
            }
            catch (Exception ex)
            {
                await Clients.Group(sessionId.ToString()).SendAsync("Error", $"An error occurred: {ex.Message}");
            }
        }

        public async Task SubmitAnswer(int sessionId, int playerId, int questionInGameId, int selectedOption, int responseTime)
        {
            try
            {
                if (sessionId <= 0 || playerId <= 0 || questionInGameId <= 0 || selectedOption < 1 || selectedOption > 4 || responseTime < 0)
                {
                    await Clients.Caller.SendAsync("Error", "Invalid input data");
                    return;
                }

                var sessionResponse = await _gameSessionService.GetGameSessionByIdAsync(sessionId);
                if (sessionResponse.StatusCode != StatusCodeEnum.OK_200 || sessionResponse.Data == null)
                {
                    await Clients.Caller.SendAsync("Error", "GameSession not found");
                    return;
                }

                var question = (await _questionService.GetQuestionByIdAsync(questionInGameId)).Data;
                if (question == null)
                {
                    await Clients.Caller.SendAsync("Error", "Invalid question");
                    return;
                }

                if (responseTime > question.TimeLimit)
                {
                    await Clients.Caller.SendAsync("Error", "Response time exceeds time limit");
                    return;
                }

                int score = CalculateScore(sessionResponse.Data, selectedOption, question.CorrectOption, responseTime, question.TimeLimit);
                int streak = await CalculateStreak(playerId, questionInGameId, selectedOption == question.CorrectOption);

                var responseRequest = new CreateResponseRequest
                {
                    PlayerId = playerId,
                    QuestionInGameId = questionInGameId,
                    SelectedOption = selectedOption,
                    ResponseTime = responseTime,
                    Score = score,
                    Streak = streak
                };

                var responseResult = await _responseService.CreateResponseAsync(responseRequest);
                if (responseResult.StatusCode != StatusCodeEnum.Created_201)
                {
                    await Clients.Caller.SendAsync("Error", "Failed to save response");
                    return;
                }

                await Clients.Caller.SendAsync("AnswerResult", new
                {
                    IsCorrect = selectedOption == question.CorrectOption,
                    Score = score,
                    Streak = streak
                });

                var players = await _gameSessionService.GetPlayersInSessionAsync(sessionId);
                if (players.StatusCode == StatusCodeEnum.OK_200 && players.Data != null)
                {
                    await Clients.Group(sessionId.ToString()).SendAsync("PlayerListUpdated", players.Data);
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", $"An error occurred: {ex.Message}");
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                var sessionId = GetSessionIdFromConnection();
                if (sessionId != null)
                {
                    await Clients.Group(sessionId).SendAsync("PlayerLeft", Context.ConnectionId);
                    var players = await _gameSessionService.GetPlayersInSessionAsync(int.Parse(sessionId));
                    if (players.StatusCode == StatusCodeEnum.OK_200 && players.Data != null)
                    {
                        await Clients.Group(sessionId).SendAsync("PlayerListUpdated", players.Data);
                    }
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", $"An error occurred on disconnect: {ex.Message}");
            }

            await base.OnDisconnectedAsync(exception);
        }

        private int CalculateScore(GameSessionResponse session, int selectedOption, int correctOption, int responseTime, int timeLimit)
        {
            int baseScore = 0;
            if (selectedOption == correctOption)
            {
                baseScore = 1000;
                if (session.EnableSpeedBonus)
                {
                    baseScore = Math.Max(0, baseScore - (responseTime * 1000 / timeLimit));
                }
            }
            return baseScore;
        }

        private async Task<int> CalculateStreak(int playerId, int questionInGameId, bool isCorrect)
        {
            var responses = await _responseService.GetResponsesByPlayerIdAsync(playerId);
            if (responses.StatusCode != StatusCodeEnum.OK_200 || responses.Data == null)
            {
                return 0;
            }

            int streak = 0;
            foreach (var response in responses.Data.OrderByDescending(r => r.ResponseId))
            {
                var question = (await _questionService.GetQuestionByIdAsync(response.QuestionInGameId)).Data;
                if (question == null) continue;

                if (response.QuestionInGameId == questionInGameId) continue;

                if (response.SelectedOption == question.CorrectOption)
                {
                    streak++;
                }
                else
                {
                    break;
                }
            }

            return isCorrect ? streak + 1 : 0;
        }

        private string GetSessionIdFromConnection()
        {
            // Lấy sessionId từ Context.Items
            if (Context.Items.TryGetValue("SessionId", out var sessionId))
            {
                return sessionId?.ToString();
            }
            return null;
        }
    }
}