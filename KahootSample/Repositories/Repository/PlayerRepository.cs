using BOs.Model;
using DAO;
using Repositories.BaseRepository;
using Repositories.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class PlayerRepository : BaseRepository<Player>, IPlayerRepository
    {
        private readonly PlayerDAO _playerDao;

        public PlayerRepository(PlayerDAO playerDao) : base(playerDao)
        {
            _playerDao = playerDao;
        }

        public async Task<Player> GetByIdAsync(int playerId)
        {
            return await _playerDao.GetByIdAsync(playerId);
        }

        public async Task<List<Player>> GetPlayersBySessionIdAsync(int sessionId)
        {
            return await _playerDao.GetPlayersBySessionIdAsync(sessionId);
        }

        public async Task<Player> AddPlayerAsync(Player player)
        {
            return await _playerDao.AddPlayerAsync(player);
        }

        public async Task<List<Player>> GetPlayersByTeamIdAsync(int teamId)
        {
            return await _playerDao.GetPlayersByTeamIdAsync(teamId);
        }

        public async Task<bool> RemovePlayerAsync(int playerId)
        {
            return await _playerDao.RemovePlayerAsync(playerId);
        }
    }
}
