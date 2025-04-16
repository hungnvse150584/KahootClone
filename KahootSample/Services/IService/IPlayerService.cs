using BOs.Model;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.PlayerRequest;
using Services.RequestAndResponse.Response;
using Services.RequestAndResponse.Response.PlayerResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface IPlayerService
    {
        Task<BaseResponse<PlayerResponse>> AddPlayerAsync(CreatePlayerRequest request);
        Task<BaseResponse<PlayerResponse>> GetPlayerByIdAsync(int playerId);
        Task<BaseResponse<IEnumerable<PlayerResponse>>> GetPlayersBySessionIdAsync(int sessionId);
        Task<BaseResponse<IEnumerable<PlayerResponse>>> GetPlayersByTeamIdAsync(int teamId);
        Task<BaseResponse<string>> RemovePlayerAsync(int playerId);
    }
}
