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

        public async Task<Question> GetByIdAsync(int id)
        {
            var question = await _context.Questions
                .Include(q => q.Answers)
                .Include(q => q.Responses)
                .FirstOrDefaultAsync(q => q.QuestionId == id);

            if (question == null)
            {
                throw new ArgumentNullException($"Question with id {id} not found");
            }

            return question;
        }

        public async Task<List<Question>> GetByQuizIdAsync(int quizId)
        {
            return await _context.Questions
                .Where(q => q.QuizId == quizId)
                .Include(q => q.Answers)
                .Include(q => q.Responses)
                .OrderBy(q => q.Order)
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
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
        }
    }
}
