using AutoMapper;
using BOs.Model;
using Repositories.IRepository;
using Services.IService;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Enum;
using Services.RequestAndResponse.Request.TeamRequest;
using Services.RequestAndResponse.Response;
using Services.RequestAndResponse.Response.TeamMemberResponses;
using Services.RequestAndResponse.Response.TeamResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IMapper _mapper;

        public TeamService(ITeamRepository teamRepository, IMapper mapper)
        {
            _teamRepository = teamRepository;
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

                // Ánh xạ từ entity sang response (TotalScore sẽ được tính động)
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

                // Ánh xạ sang response (TotalScore sẽ được tính động)
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

        public async Task<BaseResponse<TeamMemberResponse>> AddTeamMemberAsync(int teamId, int playerId)
        {
            try
            {
                // Kiểm tra đội có tồn tại
                var team = await _teamRepository.GetByIdAsync(teamId);
                if (team == null)
                {
                    return new BaseResponse<TeamMemberResponse>("Team not found", StatusCodeEnum.NotFound_404, null);
                }

                var teamMember = new TeamMember
                {
                    TeamId = teamId,
                    PlayerId = playerId,
                    Score = 0
                };

                var createdTeamMember = await _teamRepository.AddTeamMemberAsync(teamMember);
                var response = _mapper.Map<TeamMemberResponse>(createdTeamMember);

                return new BaseResponse<TeamMemberResponse>("Team member added successfully", StatusCodeEnum.Created_201, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<TeamMemberResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<IEnumerable<TeamMemberResponse>>> GetTeamMembersByTeamIdAsync(int teamId)
        {
            try
            {
                var teamMembers = await _teamRepository.GetTeamMembersByTeamIdAsync(teamId);
                if (teamMembers == null || !teamMembers.Any())
                {
                    return new BaseResponse<IEnumerable<TeamMemberResponse>>("No team members found", StatusCodeEnum.NotFound_404, null);
                }

                var response = _mapper.Map<IEnumerable<TeamMemberResponse>>(teamMembers);
                return new BaseResponse<IEnumerable<TeamMemberResponse>>("Successfully retrieved team members", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<TeamMemberResponse>>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }
    }}
