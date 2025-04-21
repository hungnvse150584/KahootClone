using BOs.Model;
using Repositories.IBaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IGameSessionRepository : IBaseRepository<GameSession>
    {
        Task<List<GameSession>> GetGameSessionsByQuizIdAsync(int quizId);
        Task<GameSession> GetGameSessionByPinAsync(string pin); // Thêm phương thức mới
        Task<List<Player>> GetPlayersInSessionAsync(int sessionId); // Thêm để hỗ trợ API GetPlayersInSession
        Task<List<Team>> GetTeamsInSessionAsync(int sessionId); // Thêm để hỗ trợ API GetTeamsInSession
    }
}
