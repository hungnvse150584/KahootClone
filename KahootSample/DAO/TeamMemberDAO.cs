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
    public class TeamMemberDAO : BaseDAO<TeamMember>
    {
        private readonly KahootDbContext _context;

        public TeamMemberDAO(KahootDbContext context) : base(context)
        {
            _context = context;
        }

        // Get TeamMember by ID with related Team and Player
        public async Task<TeamMember> GetByIdAsync(int id)
        {
            var teamMember = await _context.TeamMembers
                .Include(tm => tm.Team)
                .Include(tm => tm.Player)
                .FirstOrDefaultAsync(tm => tm.TeamMemberId == id);

            if (teamMember == null)
            {
                throw new ArgumentNullException($"TeamMember with id {id} not found");
            }

            return teamMember;
        }

        // Get TeamMembers by TeamId
        public async Task<List<TeamMember>> GetTeamMembersByTeamIdAsync(int teamId)
        {
            return await _context.TeamMembers
                .Where(tm => tm.TeamId == teamId)
                .Include(tm => tm.Team)
                .Include(tm => tm.Player)
                .ToListAsync();
        }

        // Get TeamMembers by PlayerId
        public async Task<List<TeamMember>> GetTeamMembersByPlayerIdAsync(int playerId)
        {
            return await _context.TeamMembers
                .Where(tm => tm.PlayerId == playerId)
                .Include(tm => tm.Team)
                .Include(tm => tm.Player)
                .ToListAsync();
        }
    }
}
