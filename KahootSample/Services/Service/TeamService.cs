using AutoMapper;
using BOs.Model;
using Repositories.IRepository;
using Services.IService;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Enum;
using Services.RequestAndResponse.Request.TeamRequest;
using Services.RequestAndResponse.Response;
using Services.RequestAndResponse.Response.PlayerResponse; // Thêm namespace cho PlayerResponse
using Services.RequestAndResponse.Response.TeamResponse;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Service
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IPlayerRepository _playerRepository; // Thêm IPlayerRepository để cập nhật Player
        private readonly IMapper _mapper;

        public TeamService(ITeamRepository teamRepository, IPlayerRepository playerRepository, IMapper mapper)
        {
            _teamRepository = teamRepository;
            _playerRepository = playerRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<TeamResponse>> CreateTeamAsync(CreateTeamRequest request)
        {
            try
            {
                // Ánh xạ từ request sang entity Team
                var team = _mapper.Map<Team>(request);

                // Thêm vào cơ sở dữ liệu
                var createdTeam = await _teamRepository.AddAsync(team);

                // Ánh xạ từ entity sang response
                var response = _mapper.Map<TeamResponse>(createdTeam);

                return new BaseResponse<TeamResponse>("Team created successfully", StatusCodeEnum.Created_201, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<TeamResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<TeamResponse>> UpdateTeamAsync(UpdateTeamRequest request)
        {
            try
            {
                // Lấy đội theo ID
                var team = await _teamRepository.GetByIdAsync(request.TeamId);
                if (team == null)
                {
                    return new BaseResponse<TeamResponse>("Team not found", StatusCodeEnum.NotFound_404, null);
                }

                // Cập nhật thông tin
                team.SessionId = request.SessionId;
                team.Name = request.Name;

                // Cập nhật vào cơ sở dữ liệu
                var updatedTeam = await _teamRepository.UpdateAsync(team);

                // Ánh xạ sang response
                var response = _mapper.Map<TeamResponse>(updatedTeam);

                return new BaseResponse<TeamResponse>("Team updated successfully", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<TeamResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<TeamResponse>> GetTeamByIdAsync(int teamId)
        {
            try
            {
                var team = await _teamRepository.GetByIdAsync(teamId);
                if (team == null)
                {
                    return new BaseResponse<TeamResponse>("Team not found", StatusCodeEnum.NotFound_404, null);
                }

                var response = _mapper.Map<TeamResponse>(team);
                return new BaseResponse<TeamResponse>("Successfully retrieved team", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<TeamResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<IEnumerable<TeamResponse>>> GetTeamsBySessionIdAsync(int sessionId)
        {
            try
            {
                var teams = await _teamRepository.GetTeamsBySessionIdAsync(sessionId);
                if (teams == null || !teams.Any())
                {
                    return new BaseResponse<IEnumerable<TeamResponse>>("No teams found for this session", StatusCodeEnum.NotFound_404, null);
                }

                var response = _mapper.Map<IEnumerable<TeamResponse>>(teams);
                return new BaseResponse<IEnumerable<TeamResponse>>("Successfully retrieved teams", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<TeamResponse>>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<string>> DeleteTeamAsync(int teamId)
        {
            try
            {
                var team = await _teamRepository.GetByIdAsync(teamId);
                if (team == null)
                {
                    return new BaseResponse<string>("Team not found", StatusCodeEnum.NotFound_404, null);
                }

                await _teamRepository.DeleteAsync(team);
                return new BaseResponse<string>("Team deleted successfully", StatusCodeEnum.OK_200, "Deleted successfully");
            }
            catch (Exception ex)
            {
                return new BaseResponse<string>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<PlayerResponse>> AddPlayerToTeamAsync(int teamId, int playerId)
        {
            try
            {
                // Kiểm tra đội có tồn tại
                var team = await _teamRepository.GetByIdAsync(teamId);
                if (team == null)
                {
                    return new BaseResponse<PlayerResponse>("Team not found", StatusCodeEnum.NotFound_404, null);
                }

                // Kiểm tra người chơi có tồn tại
                var player = await _playerRepository.GetByIdAsync(playerId);
                if (player == null)
                {
                    return new BaseResponse<PlayerResponse>("Player not found", StatusCodeEnum.NotFound_404, null);
                }

                // Kiểm tra xem người chơi đã ở trong đội nào chưa
                if (player.TeamId.HasValue && player.TeamId != teamId)
                {
                    return new BaseResponse<PlayerResponse>("Player is already in another team", StatusCodeEnum.BadRequest_400, null);
                }

                // Cập nhật TeamId cho người chơi
                player.TeamId = teamId;
                await _playerRepository.UpdateAsync(player);

                // Ánh xạ sang PlayerResponse
                var response = _mapper.Map<PlayerResponse>(player);

                return new BaseResponse<PlayerResponse>("Player added to team successfully", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<PlayerResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<IEnumerable<PlayerResponse>>> GetPlayersByTeamIdAsync(int teamId)
        {
            try
            {
                var players = await _teamRepository.GetPlayersByTeamIdAsync(teamId);
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
    }
}