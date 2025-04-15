using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOs.Model;
using Repositories.IBaseRepository;

namespace Repositories.IRepository
{
    public interface IScoreRepository : IBaseRepository<Score>
    {
        Task<Score?> GetByIdAsync(int id);
        Task<IEnumerable<Score>> GetBySessionIdAsync(int sessionId);
        Task AddAsync(Score score);
        Task UpdateAsync(Score score);
        Task DeleteAsync(Score score);
        Task SaveChangesAsync();
        Task<IEnumerable<Score>> GetTopScoresAsync(int topN);
        Task<int> GetTotalPointsByPlayerAsync(int playerId);
        Task<IEnumerable<Score>> GetScoresByPlayerIdAsync(int playerId);

    }
}
