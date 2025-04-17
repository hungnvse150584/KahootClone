using BOs.Model;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Response.TeamResultInGameResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface ITeamResultInGameService
    {
        Task<BaseResponse<TeamResultInGameResponse>> GetByIdAsync(int id);
        Task<BaseResponse<IEnumerable<TeamResultInGameResponse>>> GetBySessionIdAsync(int sessionId);
        Task<BaseResponse<IEnumerable<TeamResultInGameResponse>>> GetByTeamIdAsync(int teamId);
        Task<BaseResponse<int>> GetTotalTeamScoreInSessionAsync(int teamId, int sessionId);
    }
}
