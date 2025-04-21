using BOs.Model;
using DAOs;
using Repositories.BaseRepository;
using Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class GameSessionRepository : BaseRepository<GameSession>, IGameSessionRepository
    {
        private readonly GameSessionDAO _gameSessionDao;

        public GameSessionRepository(GameSessionDAO gameSessionDao) : base(gameSessionDao)
        {
            _gameSessionDao = gameSessionDao;
        }

        public async Task<List<GameSession>> GetGameSessionsByQuizIdAsync(int quizId)
        {
            return await _gameSessionDao.GetGameSessionsByQuizIdAsync(quizId);
        }

        public async Task<GameSession> GetGameSessionByPinAsync(string pin)
        {
            return await _gameSessionDao.GetGameSessionByPinAsync(pin);
        }

        public async Task<List<Player>> GetPlayersInSessionAsync(int sessionId)
        {
            return await _gameSessionDao.GetPlayersInSessionAsync(sessionId);
        }

        public async Task<List<Team>> GetTeamsInSessionAsync(int sessionId)
        {
            return await _gameSessionDao.GetTeamsInSessionAsync(sessionId);
        }
    }
}
