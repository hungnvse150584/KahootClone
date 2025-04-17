using BOs.Model;
using DAO;
using Repositories.BaseRepository;
using Repositories.IRepository;
using System.Collections.Generic;
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

        public async Task<int> GetTotalTeamScoreInSessionAsync(int teamId, int sessionId)
        {
            return await _teamResultDao.GetTotalScoreByTeamInSessionAsync(teamId, sessionId);
        }
    }
}
