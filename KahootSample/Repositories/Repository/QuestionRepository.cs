using BOs.Model;
using DAO;
using Repositories.BaseRepository;
using Repositories.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class QuestionRepository : BaseRepository<Question>, IQuestionRepository
    {
        private readonly QuestionDAO _questionDao;

        public QuestionRepository(QuestionDAO questionDao) : base(questionDao)
        {
            _questionDao = questionDao;
        }

        public async Task<List<Question>> GetByQuizIdAsync(int quizId)
        {
            return await _questionDao.GetByQuizIdAsync(quizId);
        }

        public async Task<Question> AddQuestionAsync(Question question)
        {
            return await _questionDao.AddQuestionAsync(question);
        }

        public async Task<Question> UpdateQuestionAsync(Question question)
        {
            return await _questionDao.UpdateQuestionAsync(question);
        }

        public async Task DeleteQuestionAsync(Question question)
        {
            await _questionDao.DeleteQuestionAsync(question);
        }
    }
}
