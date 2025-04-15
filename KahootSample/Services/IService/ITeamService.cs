using BOs.Model;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Request.TeamRequest;
using Services.RequestAndResponse.Response;
using Services.RequestAndResponse.TeamResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface ITeamService
    {
        Task<BaseResponse<TeamResponse>> CreateTeamAsync(CreateTeamRequest request);
        Task<BaseResponse<TeamResponse>> UpdateTeamAsync(UpdateTeamRequest request);
        Task<BaseResponse<TeamResponse>> GetTeamByIdAsync(int teamId);
        Task<BaseResponse<IEnumerable<TeamResponse>>> GetTeamsBySessionIdAsync(int sessionId);
        Task<BaseResponse<string>> DeleteTeamAsync(int teamId);
        Task<BaseResponse<TeamMemberResponse>> AddTeamMemberAsync(int teamId, int playerId);
        Task<BaseResponse<IEnumerable<TeamMemberResponse>>> GetTeamMembersByTeamIdAsync(int teamId);
    }
}
