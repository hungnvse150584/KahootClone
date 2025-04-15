using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Request.ScoreRequest;
using Services.RequestAndResponse.Response;

namespace Services.IService
{
    public interface IScoreService
    {
        Task<BaseResponse<ScoreResponse>> CreateScoreAsync(CreateScoreRequest request);
        Task<BaseResponse<ScoreResponse>> UpdateScoreAsync(UpdateScoreRequest request);
        Task<BaseResponse<string>> DeleteScoreAsync(int scoreId);
        Task<BaseResponse<ScoreResponse>> GetScoreByIdAsync(int id);
        Task<BaseResponse<IEnumerable<ScoreResponse>>> GetScoresBySessionIdAsync(int sessionId);
        Task<BaseResponse<IEnumerable<ScoreResponse>>> GetTopScoresAsync(int topN);
        Task<BaseResponse<IEnumerable<ScoreResponse>>> GetScoresByPlayerIdAsync(int playerId);
        Task<BaseResponse<int>> GetTotalPointsByPlayerAsync(int playerId);
    }

}
