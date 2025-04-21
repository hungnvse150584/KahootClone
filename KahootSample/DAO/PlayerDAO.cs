using BOs.Model;
using DAO.BaseDAO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAO
{
    public class PlayerDAO : BaseDAO<Player>
    {
        private readonly KahootDbContext _context;

        public PlayerDAO(KahootDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Player> GetByIdAsync(int playerId)
        {
            var player = await _context.Players
                .Include(p => p.User)
                .Include(p => p.Session)
                .Include(p => p.Team)
                .Include(p => p.Responses)
             
           
                .FirstOrDefaultAsync(p => p.PlayerId == playerId);

            if (player == null)
                throw new ArgumentNullException($"Player with ID {playerId} not found");

            return player;
        }

        public async Task<List<Player>> GetPlayersBySessionIdAsync(int sessionId)
        {
            return await _context.Players
                .Where(p => p.SessionId == sessionId)
                .Include(p => p.User)
                .Include(p => p.Team)
                .Include(p => p.Responses)
          
             
                .ToListAsync();
        }

        public async Task<Player> AddPlayerAsync(Player player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            await _context.Players.AddAsync(player);
            await _context.SaveChangesAsync();
            return player;
        }

        public async Task<List<Player>> GetPlayersByTeamIdAsync(int teamId)
        {
            return await _context.Players
                .Where(p => p.TeamId == teamId)
                .Include(p => p.User)
                .Include(p => p.Team)
                .Include(p => p.Responses)
        
                .ToListAsync();
        }

        public async Task<bool> RemovePlayerAsync(int playerId)
        {
            var player = await _context.Players.FindAsync(playerId);
            if (player == null)
                return false;

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}