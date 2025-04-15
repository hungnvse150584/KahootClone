using BOs.Model;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Request.ResponseRequest;
using Services.RequestAndResponse.Response.ResponseResponses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface IResponseService
    {
        Task<BaseResponse<ResponseResponse>> CreateResponseAsync(CreateResponseRequest request);
        Task<BaseResponse<ResponseResponse>> UpdateResponseAsync(int responseId, UpdateResponseRequest request);
        Task<BaseResponse<ResponseResponse>> GetResponseByIdAsync(int responseId);
        Task<BaseResponse<IEnumerable<ResponseResponse>>> GetResponsesByPlayerIdAsync(int playerId);
        Task<BaseResponse<IEnumerable<ResponseResponse>>> GetResponsesByQuestionIdAsync(int questionId);
        Task<BaseResponse<string>> DeleteResponseAsync(int responseId);
        Task<BaseResponse<IEnumerable<ResponseResponse>>> GetAllResponsesAsync();
    }
}