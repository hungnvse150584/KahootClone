using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.IBaseRepository;
using BOs.Model;

namespace Repositories.IRepository
{
    public interface IQuestionInGameRepository : IBaseRepository<QuestionInGame>
    {
        Task<List<QuestionInGame>> GetBySessionIdAsync(int sessionId);
        Task<QuestionInGame> AddQuestionInGameAsync(QuestionInGame questionInGame);
        Task<QuestionInGame> UpdateQuestionInGameAsync(QuestionInGame questionInGame);
        Task<QuestionInGame> GetQuestionInGameByIdAsync(int id); 
        Task DeleteQuestionInGameAsync(QuestionInGame questionInGame);
    }
}
