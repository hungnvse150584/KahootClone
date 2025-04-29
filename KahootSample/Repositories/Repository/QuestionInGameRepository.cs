using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOs.Model;
using DAO;
using Repositories.BaseRepository;
using Repositories.IRepository;

namespace Repositories.Repository
{
    public class QuestionInGameRepository : BaseRepository<QuestionInGame>, IQuestionInGameRepository
    {
        private readonly QuestionInGameDAO _questionInGameDao;

        public QuestionInGameRepository(QuestionInGameDAO questionInGameDao) : base(questionInGameDao)
        {
            _questionInGameDao = questionInGameDao;
        }

        public async Task<QuestionInGame> GetQuestionInGameByIdAsync(int questionInGameId)
        {
            return await _questionInGameDao.GetQuestionInGameByIdAsync(questionInGameId);
        }

        public async Task<List<QuestionInGame>> GetBySessionIdAsync(int sessionId)
        {
            return await _questionInGameDao.GetQuestionsInGameBySessionIdAsync(sessionId);
        }
        public async Task<QuestionInGame> GetQuestionInGameBySessionIdAndQuestionIdAsync(int sessionId, int questionId)
        {
            return await _questionInGameDao.GetQuestionInGameBySessionIdAndQuestionIdAsync(sessionId, questionId);
        }
        public async Task<QuestionInGame> AddQuestionInGameAsync(QuestionInGame questionInGame)
        {
            return await _questionInGameDao.AddQuestionInGameAsync(questionInGame);
        }

        public async Task<QuestionInGame> UpdateQuestionInGameAsync(QuestionInGame questionInGame)
        {
            return await _questionInGameDao.UpdateQuestionInGameAsync(questionInGame);
        }

        public async Task DeleteQuestionInGameAsync(QuestionInGame questionInGame)
        {
            await _questionInGameDao.DeleteQuestionInGameAsync(questionInGame);
        }

        public async Task<Response> GetLastResponseByPlayerIdAndSessionIdAsync(int playerId, int sessionId)
        {
            return await _questionInGameDao.GetLastResponseByPlayerIdAndSessionIdAsync(playerId, sessionId);
        }
    }
}
