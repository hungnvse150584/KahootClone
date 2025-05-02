using BOs.Model;
using DAO.BaseDAO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAO
{
    public class ResponseDAO : BaseDAO<Response>
    {
        private readonly KahootDbContext _context;

        public ResponseDAO(KahootDbContext context) : base(context)
        {
            _context = context;
        }

        // Get Response by ID with related Player and QuestionInGame
        public async Task<Response> GetByIdAsync(int id)
        {
            var response = await _context.Responses
                .Include(r => r.Player)
                .Include(r => r.QuestionInGame)
                .FirstOrDefaultAsync(r => r.ResponseId == id);

            if (response == null)
            {
                throw new ArgumentNullException($"Response with id {id} not found");
            }

            return response;
        }

        // Get Responses by PlayerId
        public async Task<List<Response>> GetResponsesByPlayerIdAsync(int playerId)
        {
            return await _context.Responses
                .Where(r => r.PlayerId == playerId)
                .Include(r => r.Player)
                .Include(r => r.QuestionInGame)
                .ToListAsync();
        }

        // Get Responses by QuestionInGameId
        public async Task<List<Response>> GetResponsesByQuestionInGameIdAsync(int questionInGameId)
        {
            return await _context.Responses
                .Where(r => r.QuestionInGameId == questionInGameId)
                .Include(r => r.Player)
                .Include(r => r.QuestionInGame)
                .ToListAsync();
        }
        public async Task<List<Response>> GetResponsesByPlayerIdAndSessionIdAsync(int playerId, int sessionId)
        {
            return await _context.Responses
                .Include(r => r.QuestionInGame)
                .Where(r => r.PlayerId == playerId && r.QuestionInGame.SessionId == sessionId)
                .ToListAsync();
        }
    }
}