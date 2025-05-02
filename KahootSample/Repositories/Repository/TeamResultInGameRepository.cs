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
    public class TeamResultInGameRepository : BaseRepository<TeamResultInGame>, ITeamResultInGameRepository
    {
        private readonly TeamResultInGameDAO _teamResultDao;

        public TeamResultInGameRepository(TeamResultInGameDAO teamResultDao) : base(teamResultDao)
        {
            _teamResultDao = teamResultDao;
        }

        public async Task<TeamResultInGame> GetByIdAsync(int id)
        {
            return await _teamResultDao.GetByIdAsync(id);
        }

        public async Task<List<TeamResultInGame>> GetBySessionIdAsync(int sessionId)
        {
            return await _teamResultDao.GetBySessionIdAsync(sessionId);
        }

        public async Task<List<TeamResultInGame>> GetByTeamIdAsync(int teamId)
        {
            return await _teamResultDao.GetByTeamIdAsync(teamId);
        }

        public async Task<TeamResultInGame> UpdateTeamResultAsync(TeamResultInGame teamResultInGame)
        {
            return await _teamResultDao.UpdateTeamResultAsync(teamResultInGame);
        }

        public async Task<List<(int TeamId, int TotalScore)>> GetTeamRankingsBySessionIdAsync(int sessionId)
        {
            return await _teamResultDao.GetTeamRankingsBySessionIdAsync(sessionId);
        }

    }
}
