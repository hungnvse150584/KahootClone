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
    public class AnswerRepository : BaseRepository<Answer>, IAnswerRepository
    {
        private readonly AnswerDAO _answerDao;

        public AnswerRepository(AnswerDAO answerDao) : base(answerDao)
        {
            _answerDao = answerDao;
        }

        public async Task<IEnumerable<Answer>> GetAllAsync()
        {
            return await _answerDao.GetAllAsync();
        }

        public async Task<Answer?> GetByIdAsync(int id)
        {
            return await _answerDao.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Answer>> GetByQuestionIdAsync(int questionId)
        {
            return await _answerDao.GetByQuestionIdAsync(questionId);
        }

        public async Task AddAsync(Answer answer)
        {
            await _answerDao.AddAsync(answer);
        }

        public async Task UpdateAsync(Answer answer)
        {
            await _answerDao.UpdateAsync(answer);
        }

        public async Task DeleteAsync(Answer answer)
        {
            await _answerDao.DeleteAsync(answer);
        }

        public async Task SaveChangesAsync()
        {
            await _answerDao.SaveChangesAsync();
        }

        public async Task<List<Answer>> GetAnswersByQuestionIdAsync(int questionId)
        {
            return await _answerDao.GetAnswersByQuestionIdAsync(questionId);
        }
    }
}
