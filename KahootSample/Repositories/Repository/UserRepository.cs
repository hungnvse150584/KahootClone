using BOs.Model;
using DAO;
using Repositories.BaseRepository;
using Repositories.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly UserDAO _userDao;

        public UserRepository(UserDAO userDao) : base(userDao)
        {
            _userDao = userDao;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _userDao.GetByUsernameAsync(username);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _userDao.GetByEmailAsync(email);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _userDao.GetAllAsync();
        }
    }
}
