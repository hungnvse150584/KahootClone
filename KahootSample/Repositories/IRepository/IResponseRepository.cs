using BOs.Model;
using Repositories.IBaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IResponseRepository : IBaseRepository<Response>
    {
        Task<List<Response>> GetResponsesByPlayerIdAsync(int playerId);
        Task<List<Response>> GetResponsesByQuestionIdAsync(int questionId);
    }
}
