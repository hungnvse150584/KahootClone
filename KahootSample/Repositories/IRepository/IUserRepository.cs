using BOs.Model;
using Repositories.IBaseRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task<List<User>> GetAllAsync();
    }
}
