using BOs.Model;
using Repositories.IBaseRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IQuizRepository : IBaseRepository<Quiz>
    {
        Task<Quiz> GetByIdAsync(int id);
        Task<List<Quiz>> GetQuizzesByUserIdAsync(int userId);
        Task<List<Quiz>> SearchQuizzesByTitleAsync(string title);
        Task<Quiz> AddQuizAsync(Quiz quiz);
        Task<Quiz> UpdateQuizAsync(Quiz quiz);
        Task<bool> DeleteQuizAsync(int quizId);
    }
}
