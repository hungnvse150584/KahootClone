using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOs.Model;
using Repositories.IBaseRepository;

namespace Repositories.IRepository
{
    public interface IAnswerRepository : IBaseRepository<Answer>
    {
        Task<IEnumerable<Answer>> GetAllAsync();
        Task<Answer?> GetByIdAsync(int id);
        Task<IEnumerable<Answer>> GetByQuestionIdAsync(int questionId);
        Task AddAsync(Answer answer);
        Task UpdateAsync(Answer answer);
        Task DeleteAsync(Answer answer);
        Task SaveChangesAsync();
        Task<List<Answer>> GetAnswersByQuestionIdAsync(int questionId);

    }
}
