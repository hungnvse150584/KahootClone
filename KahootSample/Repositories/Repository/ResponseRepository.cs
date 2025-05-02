using BOs.Model;
using DAO;
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

        public async Task<List<Response>> GetResponsesByQuestionInGameIdAsync(int questionInGameId)
        {
            return await _responseDao.GetResponsesByQuestionInGameIdAsync(questionInGameId);
        }
        public async Task<List<Response>> GetResponsesByPlayerIdAndSessionIdAsync(int playerId, int sessionId)
        {
            return await _responseDao.GetResponsesByPlayerIdAndSessionIdAsync(playerId, sessionId);
        }
    }
}