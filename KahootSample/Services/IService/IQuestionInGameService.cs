using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Request.QuestionInGameRequest;
using Services.RequestAndResponse.Response.QuestionInGameResponse;
using Services.RequestAndResponse.Response.ResponseResponses;

namespace Services.IService
{
    public interface IQuestionInGameService
    {
        Task<BaseResponse<QuestionInGameResponse>> CreateQuestionInGameAsync(CreateQuestionInGameRequest request);
        Task<BaseResponse<QuestionInGameResponse>> UpdateQuestionInGameAsync(UpdateQuestionInGameRequest request);
        Task<BaseResponse<QuestionInGameResponse>> GetQuestionInGameByIdAsync(int questionInGameId);
        Task<BaseResponse<IEnumerable<QuestionInGameResponse>>> GetQuestionsInGameBySessionIdAsync(int sessionId);
        Task<BaseResponse<string>> DeleteQuestionInGameAsync(int questionInGameId);
        Task<BaseResponse<IEnumerable<ResponseResponse>>> GetResponsesByQuestionInGameIdAsync(int questionInGameId);
        Task<BaseResponse<ResponseResponse>> GetLastResponseByPlayerIdAndSessionIdAsync(int playerId, int sessionId);
    }
}
