using BOs.Model;
using DAO.BaseDAO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAO
{
    public class TeamDAO : BaseDAO<Team>
    {
        private readonly KahootDbContext _context;

        public TeamDAO(KahootDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Team> GetByIdAsync(int id)
        {
            var team = await _context.Teams
                .Include(t => t.Players)
                .FirstOrDefaultAsync(t => t.TeamId == id);

            if (team == null)
            {
                throw new ArgumentNullException($"Team with id {id} not found");
            }

            return team;
        }

        // Get all teams in a session
        public async Task<List<Team>> GetTeamsBySessionIdAsync(int sessionId)
        {
            return await _context.Teams
                .Where(t => t.SessionId == sessionId)
                .Include(t => t.Players)
                .ToListAsync();
        }

        // Get all players in a team
        public async Task<List<Player>> GetPlayersByTeamIdAsync(int teamId)
        {
            return await _context.Players
                .Where(p => p.TeamId == teamId)
                .Include(p => p.User)
                .Include(p => p.Session)
                .Include(p => p.Team)
                .Include(p => p.Responses)
            
                
                .ToListAsync();
        }
    }
}