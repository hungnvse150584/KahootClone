using BOs.Model;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Request.TeamRequest;
using Services.RequestAndResponse.Response;
using Services.RequestAndResponse.Response.TeamResponse;
using Services.RequestAndResponse.Response.PlayerResponse; // Thêm namespace cho PlayerResponse
using System;
using System.Collections.Generic;
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
        Task<BaseResponse<PlayerResponse>> AddPlayerToTeamAsync(int teamId, int playerId);
        Task<BaseResponse<IEnumerable<PlayerResponse>>> GetPlayersByTeamIdAsync(int teamId);
    }
}