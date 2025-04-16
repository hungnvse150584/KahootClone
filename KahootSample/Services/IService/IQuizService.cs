using BOs.Model;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Request;
using Services.RequestAndResponse.Response.QuizResponses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface IQuizService
    {
        Task<BaseResponse<QuizResponse>> CreateQuizAsync(CreateQuizRequest request);
        Task<BaseResponse<QuizResponse>> UpdateQuizAsync(UpdateQuizRequest request);
        Task<BaseResponse<QuizResponse>> GetQuizByIdAsync(int quizId);
        Task<BaseResponse<IEnumerable<QuizResponse>>> GetQuizzesByUserIdAsync(int userId);
        Task<BaseResponse<IEnumerable<QuizResponse>>> SearchQuizzesByTitleAsync(string title);
        Task<BaseResponse<string>> DeleteQuizAsync(int quizId);
    }
}
