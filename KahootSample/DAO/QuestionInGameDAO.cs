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
    public class QuestionInGameDAO : BaseDAO<QuestionInGame>
    {
        private readonly KahootDbContext _context;

        public QuestionInGameDAO(KahootDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<QuestionInGame> GetQuestionInGameByIdAsync(int questionInGameId)
        {
            var questionInGame = await _context.QuestionsInGame
                .Include(q => q.Session)
                .Include(q => q.Question)
                .Include(q => q.Responses)
                .Include(q => q.TeamResults)
                .FirstOrDefaultAsync(q => q.QuestionInGameId == questionInGameId);

            if (questionInGame == null)
            {
                throw new ArgumentNullException($"QuestionInGame with id {questionInGameId} not found");
            }

            return questionInGame;
        }

        public async Task<List<QuestionInGame>> GetQuestionsInGameBySessionIdAsync(int sessionId)
        {
            return await _context.QuestionsInGame
                .Where(q => q.SessionId == sessionId)
                .Include(q => q.Session)
                .Include(q => q.Question)
                .Include(q => q.Responses)
                .Include(q => q.TeamResults)
                .OrderBy(q => q.OrderIndex)
                .ToListAsync();
        }
        public async Task<QuestionInGame> GetQuestionInGameBySessionIdAndQuestionIdAsync(int sessionId, int questionId)
        {
            var questionInGame = await _context.QuestionsInGame
                .Include(q => q.Session)
                .Include(q => q.Question)
                .FirstOrDefaultAsync(q => q.SessionId == sessionId && q.QuestionId == questionId);

            if (questionInGame == null)
            {
                throw new ArgumentNullException($"QuestionInGame with SessionId {sessionId} and QuestionId {questionId} not found");
            }

            return questionInGame;
        }
        public async Task<QuestionInGame> AddQuestionInGameAsync(QuestionInGame questionInGame)
        {
            if (questionInGame == null)
            {
                throw new ArgumentNullException(nameof(questionInGame));
            }

            await _context.QuestionsInGame.AddAsync(questionInGame);
            await _context.SaveChangesAsync();
            return questionInGame;
        }

        public async Task<QuestionInGame> UpdateQuestionInGameAsync(QuestionInGame questionInGame)
        {
            if (questionInGame == null)
            {
                throw new ArgumentNullException(nameof(questionInGame));
            }

            _context.QuestionsInGame.Update(questionInGame);
            await _context.SaveChangesAsync();
            return questionInGame;
        }

        public async Task DeleteQuestionInGameAsync(QuestionInGame questionInGame)
        {
            if (questionInGame == null)
            {
                throw new ArgumentNullException(nameof(questionInGame));
            }

            _context.QuestionsInGame.Remove(questionInGame);
            await _context.SaveChangesAsync();
        }

        public async Task<Response> GetLastResponseByPlayerIdAndSessionIdAsync(int playerId, int sessionId)
        {
            var response = await _context.Responses
                .Where(r => r.PlayerId == playerId && r.QuestionInGame.SessionId == sessionId)
                .OrderByDescending(r => r.ResponseId)
                .FirstOrDefaultAsync();

            return response;
        }
    }
}
