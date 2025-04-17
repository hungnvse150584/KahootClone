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

        public async Task<QuestionInGame> GetByIdAsync(int id)
        {
            var questionInGame = await _context.QuestionsInGame
                .Include(q => q.Question)
                .Include(q => q.Session)
                .Include(q => q.Responses)
                .Include(q => q.TeamResults)
                .FirstOrDefaultAsync(q => q.QuestionInGameId == id);

            if (questionInGame == null)
                throw new ArgumentNullException($"QuestionInGame with id {id} not found");

            return questionInGame;
        }

        public async Task<List<QuestionInGame>> GetQuestionsBySessionIdAsync(int sessionId)
        {
            return await _context.QuestionsInGame
                .Where(q => q.SessionId == sessionId)
                .Include(q => q.Question)
                .Include(q => q.Responses)
                .Include(q => q.TeamResults)
                .OrderBy(q => q.OrderIndex)
                .ToListAsync();
        }

        public async Task<QuestionInGame> AddQuestionInGameAsync(QuestionInGame questionInGame)
        {
            await _context.QuestionsInGame.AddAsync(questionInGame);
            await _context.SaveChangesAsync();
            return questionInGame;
        }

        public async Task<QuestionInGame> UpdateQuestionInGameAsync(QuestionInGame questionInGame)
        {
            _context.QuestionsInGame.Update(questionInGame);
            await _context.SaveChangesAsync();
            return questionInGame;
        }

        public async Task DeleteQuestionInGameAsync(QuestionInGame questionInGame)
        {
            _context.QuestionsInGame.Remove(questionInGame);
            await _context.SaveChangesAsync();
        }
    }
}
