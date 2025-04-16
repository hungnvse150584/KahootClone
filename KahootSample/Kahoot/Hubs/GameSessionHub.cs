using Microsoft.AspNetCore.SignalR;
using Services.IService;
using Services.RequestAndResponse.Enum;
using Services.RequestAndResponse.Request.ResponseRequest;
using Services.RequestAndResponse.Request.ScoreRequest;
using System.Text.RegularExpressions;

namespace Kahoot.Hubs
{
    public class GameSessionHub : Hub
    {
        private readonly IGameSessionService _gameSessionService;
        private readonly IQuestionService _questionService;
        private readonly IAnswerService _answerService;
        private readonly IResponseService _responseService;
        private readonly IScoreService _scoreService;

        public GameSessionHub(
            IGameSessionService gameSessionService,
            IQuestionService questionService,
            IAnswerService answerService,
            IResponseService responseService,
            IScoreService scoreService)
        {
            _gameSessionService = gameSessionService;
            _questionService = questionService;
            _answerService = answerService;
            _responseService = responseService;
            _scoreService = scoreService;
        }

        // Người chơi tham gia phiên chơi
        public async Task JoinGameSession(int sessionId, int playerId)
        {
            // Thêm người chơi vào group của phiên chơi
            await Groups.AddToGroupAsync(Context.ConnectionId, sessionId.ToString());

            // Thông báo đến tất cả người chơi trong phiên rằng có người mới tham gia
            await Clients.Group(sessionId.ToString()).SendAsync("PlayerJoined", playerId);
        }

        // Host gửi câu hỏi hiện tại đến tất cả người chơi
        public async Task SendQuestion(int sessionId, int questionId)
        {
            var question = await _questionService.GetQuestionByIdAsync(questionId);
            if (question.StatusCode != StatusCodeEnum.OK_200 || question.Data == null)
            {
                await Clients.Group(sessionId.ToString()).SendAsync("Error", "Question not found");
                return;
            }

            var answers = await _answerService.GetAnswersByQuestionIdAsync(questionId);
            if (answers.StatusCode != StatusCodeEnum.OK_200 || answers.Data == null)
            {
                await Clients.Group(sessionId.ToString()).SendAsync("Error", "Answers not found");
                return;
            }

            // Gửi câu hỏi và danh sách câu trả lời đến tất cả người chơi
            await Clients.Group(sessionId.ToString()).SendAsync("ReceiveQuestion", question.Data, answers.Data);
        }

        // Người chơi gửi câu trả lời
        public async Task SubmitAnswer(int sessionId, int playerId, int questionId, int answerId, int responseTime)
        {
            // Lưu câu trả lời của người chơi
            var answer = (await _answerService.GetAnswerByIdAsync(answerId)).Data;
            if (answer == null)
            {
                await Clients.Caller.SendAsync("Error", "Invalid answer");
                return;
            }

            // Tính điểm dựa trên độ chính xác và thời gian
            var question = (await _questionService.GetQuestionByIdAsync(questionId)).Data;
            int points = 0;
            if (answer.IsCorrect)
            {
                // Công thức tính điểm: Điểm tối đa (1000) - (Thời gian trả lời / Thời gian tối đa) * 1000
                points = Math.Max(0, 1000 - (responseTime * 1000 / question.TimeLimit));
            }

            var responseRequest = new CreateResponseRequest
            {
                PlayerId = playerId,
                QuestionId = questionId,
                AnswerId = answerId,
                ResponseTime = responseTime,
                Points = points,
                Streak = 0 // Có thể thêm logic tính streak nếu cần
            };

            await _responseService.CreateResponseAsync(responseRequest);

            // Cập nhật điểm số của người chơi
            var score = (await _scoreService.GetScoresByPlayerIdAsync(playerId)).Data?.FirstOrDefault(s => s.SessionId == sessionId);
            if (score != null)
            {
                score.TotalPoints += points;
                await _scoreService.UpdateScoreAsync(new UpdateScoreRequest { ScoreId = score.ScoreId, TotalPoints = score.TotalPoints });
            }
            else
            {
                await _scoreService.CreateScoreAsync(new CreateScoreRequest
                {
                    SessionId = sessionId,
                    PlayerId = playerId,
                    TotalPoints = points
                });
            }

            // Cập nhật bảng xếp hạng
            await UpdateLeaderboard(sessionId);
        }

        // Cập nhật bảng xếp hạng
        private async Task UpdateLeaderboard(int sessionId)
        {
            var scores = await _scoreService.GetScoresBySessionIdAsync(sessionId);
            if (scores.StatusCode == StatusCodeEnum.OK_200 && scores.Data != null)
            {
                await Clients.Group(sessionId.ToString()).SendAsync("LeaderboardUpdated", scores.Data);
            }
        }

        // Khi người chơi rời khỏi phiên
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // Xử lý khi người chơi rời (có thể thông báo đến các người chơi khác)
            await base.OnDisconnectedAsync(exception);
        }
    }
}
