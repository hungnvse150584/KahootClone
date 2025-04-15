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
    public class AnswerDAO : BaseDAO<Answer>
    {
        private readonly KahootDbContext _context;

        public AnswerDAO(KahootDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Answer>> GetAllAsync()
        {
            return await _context.Answers.Include(a => a.Question).ToListAsync();
        }

        public async Task<Answer?> GetByIdAsync(int id)
        {
            return await _context.Answers.Include(a => a.Question)
                                         .FirstOrDefaultAsync(a => a.AnswerId == id);
        }

        public async Task<IEnumerable<Answer>> GetByQuestionIdAsync(int questionId)
        {
            return await _context.Answers
                                 .Where(a => a.QuestionId == questionId)
                                 .ToListAsync();
        }

        public async Task AddAsync(Answer answer)
        {
            await _context.Answers.AddAsync(answer);
        }

        public async Task UpdateAsync(Answer answer)
        {
            _context.Answers.Update(answer);
        }

        public async Task DeleteAsync(Answer answer)
        {
            _context.Answers.Remove(answer);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<Answer>> GetAnswersByQuestionIdAsync(int questionId)
        {
            return await _context.Answers
                                 .Where(a => a.QuestionId == questionId)
                                 .ToListAsync();
        }
    }
}
