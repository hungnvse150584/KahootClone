using BOs.Model;
using Repositories.IBaseRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IQuestionRepository : IBaseRepository<Question>
    {
        Task<Question> GetQuestionByIdAsync(int questionId);
        Task<List<Question>> GetQuestionsByQuizIdAsync(int quizId);
        Task<Question> AddQuestionAsync(Question question);
        Task<Question> UpdateQuestionAsync(Question question);
        Task DeleteQuestionAsync(Question question);
        Task<Response> GetLastResponseByPlayerIdAndQuizIdAsync(int playerId, int quizId);
    }
}
