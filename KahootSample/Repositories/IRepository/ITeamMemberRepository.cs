using BOs.Model;
using Repositories.IBaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface ITeamMemberRepository : IBaseRepository<TeamMember>
    {
        Task<List<TeamMember>> GetTeamMembersByTeamIdAsync(int teamId);
        Task<List<TeamMember>> GetTeamMembersByPlayerIdAsync(int playerId);
    }
}
