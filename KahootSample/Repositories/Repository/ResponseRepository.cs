using BOs.Model;
using DAO;
using DAOs;
using Repositories.BaseRepository;
using Repositories.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class ResponseRepository : BaseRepository<Response>, IResponseRepository
    {
        private readonly ResponseDAO _responseDao;

        public ResponseRepository(ResponseDAO responseDao) : base(responseDao)
        {
            _responseDao = responseDao;
        }

        public async Task<List<Response>> GetResponsesByPlayerIdAsync(int playerId)
        {
            return await _responseDao.GetResponsesByPlayerIdAsync(playerId);
        }

        public async Task<List<Response>> GetResponsesByQuestionIdAsync(int questionId)
        {
            return await _responseDao.GetResponsesByQuestionIdAsync(questionId);
        }
    }
}