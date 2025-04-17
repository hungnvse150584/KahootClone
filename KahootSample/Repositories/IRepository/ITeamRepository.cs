using BOs.Model;
using Repositories.IBaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface ITeamRepository : IBaseRepository<Team>
    {
        Task<List<Team>> GetTeamsBySessionIdAsync(int sessionId);
        Task<List<Player>> GetPlayersByTeamIdAsync(int teamId);
    }
}