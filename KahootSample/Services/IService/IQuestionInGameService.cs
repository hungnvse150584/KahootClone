using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Request.QuestionInGameRequest;
using Services.RequestAndResponse.Response.QuestionInGameResponse;

namespace Services.IService
{
    public interface IQuestionInGameService
    {
        Task<BaseResponse<QuestionInGameResponse>> CreateAsync(CreateQuestionInGameRequest request);
        Task<BaseResponse<QuestionInGameResponse>> UpdateAsync(UpdateQuestionInGameRequest request);
        Task<BaseResponse<QuestionInGameResponse>> GetByIdAsync(int id);
        Task<BaseResponse<IEnumerable<QuestionInGameResponse>>> GetBySessionIdAsync(int sessionId);
        Task<BaseResponse<string>> DeleteAsync(int id);
    }
}
