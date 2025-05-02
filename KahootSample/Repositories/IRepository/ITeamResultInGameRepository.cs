using BOs.Model;
using Repositories.IBaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface ITeamResultInGameRepository : IBaseRepository<TeamResultInGame>
    {
        Task<TeamResultInGame> GetByIdAsync(int id);
        Task<List<TeamResultInGame>> GetBySessionIdAsync(int sessionId);
        Task<List<TeamResultInGame>> GetByTeamIdAsync(int teamId);
        Task<TeamResultInGame> UpdateTeamResultAsync(TeamResultInGame teamResultInGame);
        Task<List<(int TeamId, int TotalScore)>> GetTeamRankingsBySessionIdAsync(int sessionId);
    }
}
