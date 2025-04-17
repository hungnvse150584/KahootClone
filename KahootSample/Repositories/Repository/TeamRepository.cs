using BOs.Model;
using DAO;
using Repositories.BaseRepository;
using Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<Player>> GetPlayersByTeamIdAsync(int teamId)
        {
            return await _teamDao.GetPlayersByTeamIdAsync(teamId);
        }
    }
}