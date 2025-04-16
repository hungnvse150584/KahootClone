using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOs.Model;
using DAO.BaseDAO;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class ScoreDAO : BaseDAO<Score>
    {
        private readonly KahootDbContext _context;

        public ScoreDAO(KahootDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Score?> GetByIdAsync(int id)
        {
            return await _context.Scores.FindAsync(id);
        }

        public async Task<IEnumerable<Score>> GetBySessionIdAsync(int sessionId)
        {
            return await _context.Scores.Where(s => s.SessionId == sessionId).ToListAsync();
        }

        public async Task AddAsync(Score score)
        {
            await _context.Scores.AddAsync(score);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Score score)
        {
            _context.Scores.Update(score);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Score score)
        {
            _context.Scores.Remove(score);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task<IEnumerable<Score>> GetTopScoresAsync(int topN)
        {
            return await _context.Scores
                .OrderByDescending(s => s.TotalPoints)
                .Take(topN)
                .ToListAsync();
        }

        public async Task<int> GetTotalPointsByPlayerAsync(int playerId)
        {
            return await _context.Scores
                .Where(s => s.PlayerId == playerId)
                .SumAsync(s => s.TotalPoints);
        }
        public async Task<IEnumerable<Score>> GetScoresByPlayerIdAsync(int playerId)
        {
            return await _context.Scores
                .Where(s => s.PlayerId == playerId)
                .ToListAsync();
        }
    }

}
