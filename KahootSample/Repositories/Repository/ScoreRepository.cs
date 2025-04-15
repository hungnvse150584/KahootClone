using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOs.Model;
using DAO;
using Repositories.BaseRepository;
using Repositories.IRepository;

namespace Repositories.Repository
{
    public class ScoreRepository : BaseRepository<Score>, IScoreRepository
    {
        private readonly ScoreDAO _scoreDao;

        public ScoreRepository(ScoreDAO scoreDao) : base(scoreDao)
        {
            _scoreDao = scoreDao;
        }

        public async Task AddAsync(Score score) => await _scoreDao.AddAsync(score);
        public async Task UpdateAsync(Score score) => await _scoreDao.UpdateAsync(score);
        public async Task DeleteAsync(Score score) => await _scoreDao.DeleteAsync(score);
        public async Task<Score?> GetByIdAsync(int id) => await _scoreDao.GetByIdAsync(id);
        public async Task<IEnumerable<Score>> GetBySessionIdAsync(int sessionId) => await _scoreDao.GetBySessionIdAsync(sessionId);
        public async Task SaveChangesAsync() => await _scoreDao.SaveChangesAsync();

        public async Task<IEnumerable<Score>> GetTopScoresAsync(int topN)
        {
            return await _scoreDao.GetTopScoresAsync(topN);
        }

        public async Task<int> GetTotalPointsByPlayerAsync(int playerId)
        {
            return await _scoreDao.GetTotalPointsByPlayerAsync(playerId);
        }
        public async Task<IEnumerable<Score>> GetScoresByPlayerIdAsync(int playerId)
        {
            return await _scoreDao.GetScoresByPlayerIdAsync(playerId);
        }
    }

}
