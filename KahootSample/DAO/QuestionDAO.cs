using BOs.Model;
using DAO.BaseDAO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAO
{
    public class QuestionDAO : BaseDAO<Question>
    {
        private readonly KahootDbContext _context;

        public QuestionDAO(KahootDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Question> GetQuestionByIdAsync(int questionId)
        {
            var question = await _context.Questions
                .Include(q => q.Quiz)
                .Include(q => q.QuestionsInGame)
                .FirstOrDefaultAsync(q => q.QuestionId == questionId);

            if (question == null)
            {
                throw new ArgumentNullException($"Question with id {questionId} not found");
            }

            return question;
        }

        public async Task<List<Question>> GetQuestionsByQuizIdAsync(int quizId)
        {
            return await _context.Questions
                .Where(q => q.QuizId == quizId)
                .Include(q => q.Quiz)
                .Include(q => q.QuestionsInGame)
                .OrderBy(q => q.OrderIndex)
                .ToListAsync();
        }

        public async Task<Question> AddQuestionAsync(Question question)
        {
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }

            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task<Question> UpdateQuestionAsync(Question question)
        {
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }

            _context.Questions.Update(question);
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task DeleteQuestionAsync(Question question)
        {
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
        }

        public async Task<Response> GetLastResponseByPlayerIdAndQuizIdAsync(int playerId, int quizId)
        {
            var response = await _context.Responses
                .Include(r => r.QuestionInGame)
                .ThenInclude(qig => qig.Question)
                .Where(r => r.PlayerId == playerId && r.QuestionInGame.Question.QuizId == quizId)
                .OrderByDescending(r => r.ResponseId)
                .FirstOrDefaultAsync();

            return response;
        }
    }
}