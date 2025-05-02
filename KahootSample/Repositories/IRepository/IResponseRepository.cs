using BOs.Model;
using Repositories.IBaseRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IResponseRepository : IBaseRepository<Response>
    {
        Task<List<Response>> GetResponsesByPlayerIdAsync(int playerId);
        Task<List<Response>> GetResponsesByQuestionInGameIdAsync(int questionInGameId);
        Task<List<Response>> GetResponsesByPlayerIdAndSessionIdAsync(int playerId, int sessionId);
    }
}