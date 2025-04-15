using AutoMapper;
using BOs.Model;
using Repositories.IRepository;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Enum;
using Services.RequestAndResponse.Request.TeamMemberRequest;
using Services.RequestAndResponse.Response;
using Services.RequestAndResponse.Response.TeamMemberResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class TeamMemberService : ITeamMemberService
    {
        private readonly ITeamMemberRepository _teamMemberRepository;
        private readonly IMapper _mapper;

        public TeamMemberService(ITeamMemberRepository teamMemberRepository, IMapper mapper)
        {
            _teamMemberRepository = teamMemberRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<TeamMemberResponse>> CreateTeamMemberAsync(CreateTeamMemberRequest request)
        {
            try
            {
                var teamMember = _mapper.Map<TeamMember>(request);
                var createdTeamMember = await _teamMemberRepository.AddAsync(teamMember);
                var responseDto = _mapper.Map<TeamMemberResponse>(createdTeamMember);
                return new BaseResponse<TeamMemberResponse>("TeamMember created successfully", StatusCodeEnum.Created_201, responseDto);
            }
            catch (Exception ex)
            {
                return new BaseResponse<TeamMemberResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<TeamMemberResponse>> UpdateTeamMemberAsync(int teamMemberId, UpdateTeamMemberRequest request)
        {
            try
            {
                var teamMember = await _teamMemberRepository.GetByIdAsync(teamMemberId);
                if (teamMember == null)
                {
                    return new BaseResponse<TeamMemberResponse>($"TeamMember with id {teamMemberId} not found", StatusCodeEnum.NotFound_404, null);
                }

                teamMember.TeamId = request.TeamId;
                teamMember.PlayerId = request.PlayerId;
                teamMember.Score = request.Score;

                var updatedTeamMember = await _teamMemberRepository.UpdateAsync(teamMember);
                var responseDto = _mapper.Map<TeamMemberResponse>(updatedTeamMember);

                return new BaseResponse<TeamMemberResponse>("TeamMember updated successfully", StatusCodeEnum.OK_200, responseDto);
            }
            catch (ArgumentNullException ex)
            {
                return new BaseResponse<TeamMemberResponse>(ex.Message, StatusCodeEnum.NotFound_404, null);
            }
            catch (Exception ex)
            {
                return new BaseResponse<TeamMemberResponse>($"An error occurred while updating TeamMember: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<TeamMemberResponse>> GetTeamMemberByIdAsync(int teamMemberId)
        {
            try
            {
                var teamMember = await _teamMemberRepository.GetByIdAsync(teamMemberId);
                if (teamMember == null)
                {
                    return new BaseResponse<TeamMemberResponse>("TeamMember not found", StatusCodeEnum.NotFound_404, null);
                }

                var responseDto = _mapper.Map<TeamMemberResponse>(teamMember);
                return new BaseResponse<TeamMemberResponse>("Successfully retrieved TeamMember", StatusCodeEnum.OK_200, responseDto);
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
                var teamMembers = await _teamMemberRepository.GetTeamMembersByTeamIdAsync(teamId);
                if (teamMembers == null || !teamMembers.Any())
                {
                    return new BaseResponse<IEnumerable<TeamMemberResponse>>("No TeamMembers found for this team", StatusCodeEnum.NotFound_404, null);
                }

                var responseDtos = _mapper.Map<IEnumerable<TeamMemberResponse>>(teamMembers);
                return new BaseResponse<IEnumerable<TeamMemberResponse>>("Successfully retrieved TeamMembers", StatusCodeEnum.OK_200, responseDtos);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<TeamMemberResponse>>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<IEnumerable<TeamMemberResponse>>> GetTeamMembersByPlayerIdAsync(int playerId)
        {
            try
            {
                var teamMembers = await _teamMemberRepository.GetTeamMembersByPlayerIdAsync(playerId);
                if (teamMembers == null || !teamMembers.Any())
                {
                    return new BaseResponse<IEnumerable<TeamMemberResponse>>("No TeamMembers found for this player", StatusCodeEnum.NotFound_404, null);
                }

                var responseDtos = _mapper.Map<IEnumerable<TeamMemberResponse>>(teamMembers);
                return new BaseResponse<IEnumerable<TeamMemberResponse>>("Successfully retrieved TeamMembers", StatusCodeEnum.OK_200, responseDtos);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<TeamMemberResponse>>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<string>> DeleteTeamMemberAsync(int teamMemberId)
        {
            try
            {
                var teamMember = await _teamMemberRepository.GetByIdAsync(teamMemberId);
                if (teamMember == null)
                {
                    return new BaseResponse<string>("TeamMember not found", StatusCodeEnum.NotFound_404, null);
                }

                await _teamMemberRepository.DeleteAsync(teamMember);
                return new BaseResponse<string>("TeamMember deleted successfully", StatusCodeEnum.OK_200, "Deleted successfully");
            }
            catch (Exception ex)
            {
                return new BaseResponse<string>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<IEnumerable<TeamMemberResponse>>> GetAllTeamMembersAsync()
        {
            try
            {
                var teamMembers = await _teamMemberRepository.GetAllAsync();
                if (teamMembers == null || !teamMembers.Any())
                {
                    return new BaseResponse<IEnumerable<TeamMemberResponse>>("No TeamMembers found", StatusCodeEnum.NotFound_404, null);
                }

                var responseDtos = _mapper.Map<IEnumerable<TeamMemberResponse>>(teamMembers);
                return new BaseResponse<IEnumerable<TeamMemberResponse>>("Successfully retrieved all TeamMembers", StatusCodeEnum.OK_200, responseDtos);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<TeamMemberResponse>>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }
    }
}
