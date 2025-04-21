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

        public async Task<Question> GetQuestionByIdAsync(int questionId)
        {
            return await _questionDao.GetQuestionByIdAsync(questionId);
        }

        public async Task<List<Question>> GetQuestionsByQuizIdAsync(int quizId)
        {
            return await _questionDao.GetQuestionsByQuizIdAsync(quizId);
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

        public async Task<Response> GetLastResponseByPlayerIdAndQuizIdAsync(int playerId, int quizId)
        {
            return await _questionDao.GetLastResponseByPlayerIdAndQuizIdAsync(playerId, quizId);
        }
    }
}
