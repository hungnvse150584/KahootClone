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
    public class TeamDAO : BaseDAO<Team>
    {
        private readonly KahootDbContext _context;

        public TeamDAO(KahootDbContext context) : base(context)
        {
            _context = context;
        }

        public  async Task<Team> GetByIdAsync(int id)
        {
            var team = await _context.Teams
                .Include(t => t.TeamMembers)
                .ThenInclude(tm => tm.Player)
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
                .Include(t => t.TeamMembers)
                .ThenInclude(tm => tm.Player)
                .ToListAsync();
        }

        // TeamMember-specific operations
        public async Task<TeamMember> AddTeamMemberAsync(TeamMember teamMember)
        {
            if (teamMember == null)
            {
                throw new ArgumentNullException(nameof(teamMember));
            }

            await _context.TeamMembers.AddAsync(teamMember);
            await _context.SaveChangesAsync();
            return teamMember;
        }

        public async Task<List<TeamMember>> GetTeamMembersByTeamIdAsync(int teamId)
        {
            return await _context.TeamMembers
                .Where(tm => tm.TeamId == teamId)
                .Include(tm => tm.Player)
                .ToListAsync();
        }
    }
}
