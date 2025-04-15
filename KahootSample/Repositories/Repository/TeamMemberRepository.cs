using BOs.Model;
using DAO;
using DAOs;
using Repositories.BaseRepository;
using Repositories.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class TeamMemberRepository : BaseRepository<TeamMember>, ITeamMemberRepository
    {
        private readonly TeamMemberDAO _teamMemberDao;

        public TeamMemberRepository(TeamMemberDAO teamMemberDao) : base(teamMemberDao)
        {
            _teamMemberDao = teamMemberDao;
        }

        public async Task<List<TeamMember>> GetTeamMembersByTeamIdAsync(int teamId)
        {
            return await _teamMemberDao.GetTeamMembersByTeamIdAsync(teamId);
        }

        public async Task<List<TeamMember>> GetTeamMembersByPlayerIdAsync(int playerId)
        {
            return await _teamMemberDao.GetTeamMembersByPlayerIdAsync(playerId);
        }
    }
}