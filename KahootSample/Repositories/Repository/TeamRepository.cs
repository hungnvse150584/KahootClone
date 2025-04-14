using BOs.Model;
using DAO;
using Repositories.BaseRepository;
using Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class TeamRepository : BaseRepository<Team>, ITeamRepository
    {
        private readonly TeamDAO _teamDao;

        public TeamRepository(TeamDAO teamDao) : base(teamDao)
        {
            _teamDao = teamDao;
        }

        public async Task<List<Team>> GetTeamsBySessionIdAsync(int sessionId)
        {
            return await _teamDao.GetTeamsBySessionIdAsync(sessionId);
        }

        public async Task<TeamMember> AddTeamMemberAsync(TeamMember teamMember)
        {
            return await _teamDao.AddTeamMemberAsync(teamMember);
        }

        public async Task<List<TeamMember>> GetTeamMembersByTeamIdAsync(int teamId)
        {
            return await _teamDao.GetTeamMembersByTeamIdAsync(teamId);
        }
    }
}
