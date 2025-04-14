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
        Task<TeamMember> AddTeamMemberAsync(TeamMember teamMember);
        Task<List<TeamMember>> GetTeamMembersByTeamIdAsync(int teamId);
    }
}
