using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Request.ResponseRequest;
using Services.RequestAndResponse.Response;
using Services.RequestAndResponse.Response.ResponseResponses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface IResponseService
    {
        Task<BaseResponse<ResponseResponse>> CreateResponseAsync(CreateResponseRequest request);
        Task<BaseResponse<ResponseResponse>> GetResponseByIdAsync(int responseId); // Thêm phương thức này
        Task<BaseResponse<IEnumerable<ResponseResponse>>> GetAllResponsesAsync(); // Thêm phương thức này
        Task<BaseResponse<ResponseResponse>> UpdateResponseAsync(int responseId, UpdateResponseRequest request);
        Task<BaseResponse<string>> DeleteResponseAsync(int responseId); // Thêm phương thức này
        Task<BaseResponse<IEnumerable<ResponseResponse>>> GetResponsesByQuestionInGameIdAsync(int questionInGameId);
        Task<BaseResponse<IEnumerable<ResponseResponse>>> GetResponsesByPlayerIdAsync(int playerId);
        Task<BaseResponse<IEnumerable<ResponseResponse>>> GetResponsesByPlayerIdAndSessionIdAsync(int playerId, int sessionId);
    }
}