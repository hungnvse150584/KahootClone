using BOs.Model;
using DAO.BaseDAO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class TeamResultInGameDAO : BaseDAO<TeamResultInGame>
    {
        private readonly KahootDbContext _context;

        public TeamResultInGameDAO(KahootDbContext context) : base(context)
        {
            _context = context;
        }

        // Get TeamResultInGame by ID
        public async Task<TeamResultInGame> GetByIdAsync(int id)
        {
            var teamResult = await _context.TeamResults
                .Include(tr => tr.QuestionInGame)
                    .ThenInclude(qig => qig.Question) // Bao gồm thông tin câu hỏi
                .Include(tr => tr.Session)        // Bao gồm thông tin phiên chơi
                .Include(tr => tr.Team)           // Bao gồm thông tin đội
                .FirstOrDefaultAsync(tr => tr.TeamResultInGameId == id);

            if (teamResult == null)
            {
                throw new ArgumentNullException($"TeamResultInGame with id {id} not found");
            }

            return teamResult;
        }

        // Get all TeamResultInGame records by SessionId
        public async Task<List<TeamResultInGame>> GetBySessionIdAsync(int sessionId)
        {
            return await _context.TeamResults
                .Where(tr => tr.SessionId == sessionId)
                .Include(tr => tr.QuestionInGame)
                    .ThenInclude(qig => qig.Question)
                .Include(tr => tr.Session)
                .Include(tr => tr.Team)
                .ToListAsync();
        }

        // Get all TeamResultInGame records by TeamId
        public async Task<List<TeamResultInGame>> GetByTeamIdAsync(int teamId)
        {
            return await _context.TeamResults
                .Where(tr => tr.TeamId == teamId)
                .Include(tr => tr.QuestionInGame)
                    .ThenInclude(qig => qig.Question)
                .Include(tr => tr.Session)
                .Include(tr => tr.Team)
                .ToListAsync();
        }

        public async Task<List<(int TeamId, int TotalScore)>> GetTeamRankingsBySessionIdAsync(int sessionId)
        {
            var result = await _context.TeamResults
                .Where(tr => tr.SessionId == sessionId)
                .GroupBy(tr => tr.TeamId)
                .Select(g => new { TeamId = g.Key, TotalScore = g.Sum(tr => tr.Score) })
                .OrderByDescending(g => g.TotalScore)
                .ToListAsync();
            return result.Select(x => (x.TeamId, x.TotalScore)).ToList();
        }


    }
}
