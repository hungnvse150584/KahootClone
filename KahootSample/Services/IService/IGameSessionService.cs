using BOs.Model;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Request.GameSessionRequest;
using Services.RequestAndResponse.Response.GameSessionResponses;
using Services.RequestAndResponse.Response.PlayerResponse;
using Services.RequestAndResponse.Response.TeamResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface IGameSessionService
    {
        Task<BaseResponse<GameSessionResponse>> CreateGameSessionAsync(CreateGameSessionRequest request);
        Task<BaseResponse<GameSessionResponse>> UpdateGameSessionAsync(int sessionId, UpdateGameSessionRequest request);
        Task<BaseResponse<GameSessionResponse>> GetGameSessionByIdAsync(int sessionId);
        Task<BaseResponse<IEnumerable<GameSessionResponse>>> GetGameSessionsByQuizIdAsync(int quizId);
        Task<BaseResponse<string>> DeleteGameSessionAsync(int sessionId);
        Task<BaseResponse<IEnumerable<GameSessionResponse>>> GetAllGameSessionsAsync();
        Task<BaseResponse<GameSessionResponse>> GetGameSessionByPinAsync(string pin); // Thêm API mới
        Task<BaseResponse<IEnumerable<PlayerResponse>>> GetPlayersInSessionAsync(int sessionId); // Thêm API mới
        Task<BaseResponse<IEnumerable<TeamResponse>>> GetTeamsInSessionAsync(int sessionId); // Thêm API mới
        Task<BaseResponse<string>> EndGameSessionAsync(int sessionId); // Thêm API mới
    }
}