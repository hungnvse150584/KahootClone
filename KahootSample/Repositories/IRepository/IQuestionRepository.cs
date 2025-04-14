using BOs.Model;
using Repositories.IBaseRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IQuestionRepository : IBaseRepository<Question>
    {
        Task<List<Question>> GetByQuizIdAsync(int quizId);
        Task<Question> AddQuestionAsync(Question question);
        Task<Question> UpdateQuestionAsync(Question question);
        Task DeleteQuestionAsync(Question question);
    }
}
