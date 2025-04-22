using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Request.TeamResultRequest;
using Services.RequestAndResponse.Response.TeamResultResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface ITeamResultInGameService
    {
        Task<BaseResponse<TeamResultResponse>> GetTeamResultByIdAsync(int teamResultId);
        Task<BaseResponse<IEnumerable<TeamResultResponse>>> GetTeamResultsBySessionIdAsync(int sessionId);
        Task<BaseResponse<IEnumerable<TeamResultResponse>>> GetTeamResultsByTeamIdAsync(int teamId);
        Task<BaseResponse<TeamResultResponse>> CreateTeamResultAsync(CreateTeamResultRequest request);
        Task<BaseResponse<TeamResultResponse>> UpdateTeamResultAsync(UpdateTeamResultRequest request);
        Task<BaseResponse<string>> DeleteTeamResultAsync(int teamResultId);
    }
}
