using BOs.Model;
using DAO;
using DAO.BaseDAO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAOs
{
    public class GameSessionDAO : BaseDAO<GameSession>
    {
        private readonly KahootDbContext _context;

        public GameSessionDAO(KahootDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<GameSession> GetByIdAsync(int id)
        {
            var gameSession = await _context.GameSessions
                .Include(gs => gs.Quiz)
                .Include(gs => gs.Players)
                .Include(gs => gs.Teams)
                .FirstOrDefaultAsync(gs => gs.SessionId == id);

            if (gameSession == null)
            {
                throw new ArgumentNullException($"GameSession with id {id} not found");
            }

            return gameSession;
        }

        public async Task<List<GameSession>> GetGameSessionsByQuizIdAsync(int quizId)
        {
            return await _context.GameSessions
                .Where(gs => gs.QuizId == quizId)
                .Include(gs => gs.Quiz)
                .Include(gs => gs.Players)
                .Include(gs => gs.Teams)
                .ToListAsync();
        }

        public async Task<GameSession> GetGameSessionByPinAsync(string pin)
        {
            var gameSession = await _context.GameSessions
                .Include(gs => gs.Quiz)
                .Include(gs => gs.Players)
                .Include(gs => gs.Teams)
                .FirstOrDefaultAsync(gs => gs.Pin == pin);

            if (gameSession == null)
            {
                throw new ArgumentNullException($"GameSession with pin {pin} not found");
            }

            return gameSession;
        }

        public async Task<List<Player>> GetPlayersInSessionAsync(int sessionId)
        {
            return await _context.Players
                .Where(p => p.SessionId == sessionId)
                .Include(p => p.User)
                .Include(p => p.Team)
                .ToListAsync();
        }

        public async Task<List<Team>> GetTeamsInSessionAsync(int sessionId)
        {
            return await _context.Teams
                .Where(t => t.SessionId == sessionId)
                .Include(t => t.Players)
                .ToListAsync();
        }
    }
}