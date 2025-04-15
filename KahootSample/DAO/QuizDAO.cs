using BOs.Model;
using DAO.BaseDAO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAO
{
    public class QuizDAO : BaseDAO<Quiz>
    {
        private readonly KahootDbContext _context;

        public QuizDAO(KahootDbContext context) : base(context)
        {
            _context = context;
        }

        // Get quiz by ID with related questions and sessions
        public async Task<Quiz> GetByIdAsync(int id)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .Include(q => q.GameSessions)
                .FirstOrDefaultAsync(q => q.QuizId == id);

            if (quiz == null)
            {
                throw new ArgumentNullException($"Quiz with id {id} not found");
            }

            return quiz;
        }

        // Get all quizzes created by a specific user
        public async Task<List<Quiz>> GetQuizzesByUserIdAsync(int userId)
        {
            return await _context.Quizzes
                .Where(q => q.CreatedBy == userId)
                .Include(q => q.Questions)
                .ToListAsync();
        }

        // Add a new quiz
        public async Task<Quiz> AddQuizAsync(Quiz quiz)
        {
            if (quiz == null)
            {
                throw new ArgumentNullException(nameof(quiz));
            }

            await _context.Quizzes.AddAsync(quiz);
            await _context.SaveChangesAsync();
            return quiz;
        }

        // Delete a quiz
        public async Task<bool> DeleteQuizAsync(int quizId)
        {
            var quiz = await _context.Quizzes.FindAsync(quizId);
            if (quiz == null)
                return false;

            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();
            return true;
        }

        // Update a quiz
        public async Task<Quiz> UpdateQuizAsync(Quiz updatedQuiz)
        {
            var existingQuiz = await _context.Quizzes.FindAsync(updatedQuiz.QuizId);
            if (existingQuiz == null)
                throw new ArgumentNullException($"Quiz with id {updatedQuiz.QuizId} not found");

            _context.Entry(existingQuiz).CurrentValues.SetValues(updatedQuiz);
            await _context.SaveChangesAsync();
            return updatedQuiz;
        }

        // Search quizzes by title
        public async Task<List<Quiz>> SearchQuizzesByTitleAsync(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return new List<Quiz>();
            }

            return await _context.Quizzes
                .Where(q => EF.Functions.Like(q.Title.ToLower(), $"%{title.ToLower()}%"))
                .Include(q => q.Questions)
                .ToListAsync();
        }
    }
}
