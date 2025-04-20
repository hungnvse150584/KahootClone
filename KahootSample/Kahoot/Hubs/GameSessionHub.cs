using Microsoft.AspNetCore.SignalR;
using Services.IService;
using Services.RequestAndResponse.Enum;
using Services.RequestAndResponse.Request.ResponseRequest;

using System.Text.RegularExpressions;
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
            IResponseService responseService
           )
        {
            _gameSessionService = gameSessionService;
            _questionService = questionService;
            _responseService = responseService;
        
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

            // Gửi câu hỏi đến tất cả người chơi (không cần Answers vì đã xóa bảng Answer)
            await Clients.Group(sessionId.ToString()).SendAsync("ReceiveQuestion", question.Data);
        }

        // Người chơi gửi câu trả lời
        public async Task SubmitAnswer(int sessionId, int playerId, int questionInGameId, int selectedOption, int responseTime)
        {
            // Lấy câu hỏi để tính điểm
            var question = (await _questionService.GetQuestionByIdAsync(questionInGameId)).Data;
            if (question == null)
            {
                await Clients.Caller.SendAsync("Error", "Invalid question");
                return;
            }

            // Tính điểm dựa trên độ chính xác và thời gian
            int score = 0;
            if (selectedOption == question.CorrectOption)
            {
                // Công thức tính điểm: Điểm tối đa (1000) - (Thời gian trả lời / Thời gian tối đa) * 1000
                score = Math.Max(0, 1000 - (responseTime * 1000 / question.TimeLimit));
            }

            var responseRequest = new CreateResponseRequest
            {
                PlayerId = playerId,
                QuestionInGameId = questionInGameId, // Sửa từ QuestionId thành QuestionInGameId
                SelectedOption = selectedOption, // Sửa từ AnswerId thành SelectedOption
                ResponseTime = responseTime,
                Score = score, // Sửa từ Points thành Score
                Streak = 0 // Có thể thêm logic tính streak nếu cần
            };

            await _responseService.CreateResponseAsync(responseRequest);

            // Cập nhật điểm số của người chơi
            //var existingScore = (await _scoreService.GetScoresByPlayerIdAsync(playerId)).Data?.FirstOrDefault(s => s.SessionId == sessionId);
            //if (existingScore != null)
            //{
            //    existingScore.TotalPoints += score;
            //    await _scoreService.UpdateScoreAsync(new UpdateScoreRequest { ScoreId = existingScore.ScoreId, TotalPoints = existingScore.TotalPoints });
            //}
            //else
            //{
            //    await _scoreService.CreateScoreAsync(new CreateScoreRequest
            //    {
            //        SessionId = sessionId,
            //        PlayerId = playerId,
            //        TotalPoints = score
            //    });
            //}

            // Cập nhật bảng xếp hạng
            //await UpdateLeaderboard(sessionId);
        }

        // Cập nhật bảng xếp hạng
        //private async Task UpdateLeaderboard(int sessionId)
        //{
        //    var scores = await _scoreService.GetScoresBySessionIdAsync(sessionId);
        //    if (scores.StatusCode == StatusCodeEnum.OK_200 && scores.Data != null)
        //    {
        //        await Clients.Group(sessionId.ToString()).SendAsync("LeaderboardUpdated", scores.Data);
        //    }
        //}

        // Khi người chơi rời khỏi phiên
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // Xử lý khi người chơi rời (có thể thông báo đến các người chơi khác)
            await base.OnDisconnectedAsync(exception);
        }
    }
}