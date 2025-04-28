using BOs.Model;
using DAO;
using Repositories.BaseRepository;
using Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class QuizRepository : BaseRepository<Quiz>, IQuizRepository
    {
        private readonly QuizDAO _quizDao;

        public QuizRepository(QuizDAO quizDao) : base(quizDao)
        {
            _quizDao = quizDao;
        }

        public async Task<List<Quiz>> GetAllQuizzesAsync()
        {
            return await _quizDao.GetAllQuizzesAsync();
        }


        public async Task<Quiz> GetByIdAsync(int id)
        {
            return await _quizDao.GetByIdAsync(id);
        }

        public async Task<List<Quiz>> GetQuizzesByUserIdAsync(int userId)
        {
            return await _quizDao.GetQuizzesByUserIdAsync(userId);
        }

        public async Task<List<Quiz>> SearchQuizzesByTitleAsync(string title)
        {
            return await _quizDao.SearchQuizzesByTitleAsync(title);
        }

        public async Task<Quiz> AddQuizAsync(Quiz quiz)
        {
            return await _quizDao.AddQuizAsync(quiz);
        }

        public async Task<Quiz> UpdateQuizAsync(Quiz quiz)
        {
            return await _quizDao.UpdateQuizAsync(quiz);
        }

        public async Task<bool> DeleteQuizAsync(int quizId)
        {
            return await _quizDao.DeleteQuizAsync(quizId);
        }
    }
}
