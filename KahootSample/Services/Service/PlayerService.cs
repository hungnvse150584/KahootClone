using AutoMapper;
using BOs.Model;
using Repositories.IRepository;
using Services.IService;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Enum;
using Services.RequestAndResponse.PlayerRequest;
using Services.RequestAndResponse.Response.PlayerResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Service
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IMapper _mapper;

        public PlayerService(IPlayerRepository playerRepository, IMapper mapper)
        {
            _playerRepository = playerRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<PlayerResponse>> AddPlayerAsync(CreatePlayerRequest request)
        {
            try
            {
                var player = _mapper.Map<Player>(request);
                var createdPlayer = await _playerRepository.AddPlayerAsync(player);
                var response = _mapper.Map<PlayerResponse>(createdPlayer);

                return new BaseResponse<PlayerResponse>("Player created successfully", StatusCodeEnum.Created_201, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<PlayerResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<PlayerResponse>> GetPlayerByIdAsync(int playerId)
        {
            try
            {
                var player = await _playerRepository.GetByIdAsync(playerId);
                if (player == null)
                {
                    return new BaseResponse<PlayerResponse>("Player not found", StatusCodeEnum.NotFound_404, null);
                }

                var response = _mapper.Map<PlayerResponse>(player);
                return new BaseResponse<PlayerResponse>("Successfully retrieved player", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<PlayerResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<IEnumerable<PlayerResponse>>> GetPlayersBySessionIdAsync(int sessionId)
        {
            try
            {
                var players = await _playerRepository.GetPlayersBySessionIdAsync(sessionId);
                if (players == null || !players.Any())
                {
                    return new BaseResponse<IEnumerable<PlayerResponse>>("No players found for this session", StatusCodeEnum.NotFound_404, null);
                }

                var response = _mapper.Map<IEnumerable<PlayerResponse>>(players);
                return new BaseResponse<IEnumerable<PlayerResponse>>("Successfully retrieved players", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<PlayerResponse>>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<IEnumerable<PlayerResponse>>> GetPlayersByTeamIdAsync(int teamId)
        {
            try
            {
                var players = await _playerRepository.GetPlayersByTeamIdAsync(teamId);
                if (players == null || !players.Any())
                {
                    return new BaseResponse<IEnumerable<PlayerResponse>>("No players found in this team", StatusCodeEnum.NotFound_404, null);
                }

                var response = _mapper.Map<IEnumerable<PlayerResponse>>(players);
                return new BaseResponse<IEnumerable<PlayerResponse>>("Successfully retrieved players", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<PlayerResponse>>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<string>> RemovePlayerAsync(int playerId)
        {
            try
            {
                var success = await _playerRepository.RemovePlayerAsync(playerId);
                if (!success)
                {
                    return new BaseResponse<string>("Player not found", StatusCodeEnum.NotFound_404, null);
                }

                return new BaseResponse<string>("Player removed successfully", StatusCodeEnum.OK_200, "Deleted");
            }
            catch (Exception ex)
            {
                return new BaseResponse<string>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }
    }
}
