using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Request.QuestionRequest;
using Services.RequestAndResponse.Response.QuestionResponses;
using Services.RequestAndResponse.Response.ResponseResponses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface IQuestionService
    {
        Task<BaseResponse<QuestionResponse>> CreateQuestionAsync(CreateQuestionRequest request);
        Task<BaseResponse<QuestionResponse>> UpdateQuestionAsync(UpdateQuestionRequest request);
        Task<BaseResponse<IEnumerable<QuestionResponse>>> GetAllQuestionsAsync();
        Task<BaseResponse<QuestionResponse>> GetQuestionByIdAsync(int questionId);
        Task<BaseResponse<IEnumerable<QuestionResponse>>> GetQuestionsByQuizIdAsync(int quizId);
        Task<BaseResponse<string>> DeleteQuestionAsync(int questionId);
        Task<BaseResponse<IEnumerable<ResponseResponse>>> GetResponsesByQuestionInGameIdAsync(int questionInGameId);
        Task<BaseResponse<ResponseResponse>> GetLastResponseByPlayerIdAndQuizIdAsync(int playerId, int quizId);
        Task<BaseResponse<IEnumerable<QuestionResponse>>> SearchQuestionsAsync(int? quizId, int? sessionId, string searchTerm);
    }
}
