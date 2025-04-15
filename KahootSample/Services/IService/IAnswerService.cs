using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Request.AnswerRequest;
using Services.RequestAndResponse.Response;

namespace Services.IService
{
    public interface IAnswerService
    {
        Task<BaseResponse<AnswerResponse>> CreateAnswerAsync(CreateAnswerRequest request);
        Task<BaseResponse<AnswerResponse>> UpdateAnswerAsync(UpdateAnswerRequest request);
        Task<BaseResponse<AnswerResponse>> GetAnswerByIdAsync(int answerId);
        Task<BaseResponse<IEnumerable<AnswerResponse>>> GetAnswersByQuestionIdAsync(int questionId);
        Task<BaseResponse<string>> DeleteAnswerAsync(int answerId);
    }
}
