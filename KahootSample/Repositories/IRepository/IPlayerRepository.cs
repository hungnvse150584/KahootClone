﻿using BOs.Model;
using Repositories.IBaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IPlayerRepository : IBaseRepository<Player>
    {
        Task<List<Player>> GetAllPlayersAsync();
        Task<Player> GetByIdAsync(int playerId);
        Task<List<Player>> GetPlayersBySessionIdAsync(int sessionId);
        Task<Player> AddPlayerAsync(Player player);
        Task<List<Player>> GetPlayersByTeamIdAsync(int teamId);
        Task<bool> RemovePlayerAsync(int playerId);
        Task<Player> UpdatePlayerAsync(Player player);
    }
}
