using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Request.QuestionRequest;
using Services.RequestAndResponse.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface IQuestionService
    {
        Task<BaseResponse<QuestionResponse>> CreateQuestionAsync(CreateQuestionRequest request);
        Task<BaseResponse<QuestionResponse>> UpdateQuestionAsync(UpdateQuestionRequest request);
        Task<BaseResponse<QuestionResponse>> GetQuestionsAsync();
        Task<BaseResponse<QuestionResponse>> GetQuestionByIdAsync(int questionId);
        Task<BaseResponse<IEnumerable<QuestionResponse>>> GetQuestionsByQuizIdAsync(int quizId);
        Task<BaseResponse<string>> DeleteQuestionAsync(int questionId);
    }
}
