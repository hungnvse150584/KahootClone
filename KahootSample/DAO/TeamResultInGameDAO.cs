using BOs.Model;
using DAO.BaseDAO;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        // Get by ID
        public async Task<TeamResultInGame> GetByIdAsync(int id)
        {
            var teamResult = await _context.TeamResults
                .Include(tr => tr.QuestionInGame)
                .Include(tr => tr.Player)
                .Include(tr => tr.Session)
                .Include(tr => tr.Team)
                .FirstOrDefaultAsync(tr => tr.TeamResultInGameId == id);

            if (teamResult == null)
            {
                throw new ArgumentNullException($"Team result with id {id} not found");
            }

            return teamResult;
        }

        // Get all results by SessionId
        public async Task<List<TeamResultInGame>> GetBySessionIdAsync(int sessionId)
        {
            return await _context.TeamResults
                .Where(tr => tr.SessionId == sessionId)
                .Include(tr => tr.Team)
                .Include(tr => tr.QuestionInGame)
                .Include(tr => tr.Player)
                .ToListAsync();
        }

        // Get all results by TeamId
        public async Task<List<TeamResultInGame>> GetByTeamIdAsync(int teamId)
        {
            return await _context.TeamResults
                .Where(tr => tr.TeamId == teamId)
                .Include(tr => tr.QuestionInGame)
                .Include(tr => tr.Player)
                .Include(tr => tr.Session)
                .ToListAsync();
        }


        // Get score summary for a team in a session
        public async Task<int> GetTotalScoreByTeamInSessionAsync(int teamId, int sessionId)
        {
            return await _context.TeamResults
                .Where(tr => tr.TeamId == teamId && tr.SessionId == sessionId)
                .SumAsync(tr => tr.Score);
        }
    }
}
