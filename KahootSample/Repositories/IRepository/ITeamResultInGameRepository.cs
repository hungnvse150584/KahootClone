using BOs.Model;
using Repositories.IBaseRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface ITeamResultInGameRepository : IBaseRepository<TeamResultInGame>
    {
        Task<TeamResultInGame> GetByIdAsync(int id);
        Task<List<TeamResultInGame>> GetBySessionIdAsync(int sessionId);
        Task<List<TeamResultInGame>> GetByTeamIdAsync(int teamId);

        // Tổng điểm của cả đội trong một phiên chơi (session)
        Task<int> GetTotalTeamScoreInSessionAsync(int teamId, int sessionId);
    }
}
