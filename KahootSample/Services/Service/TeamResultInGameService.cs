using AutoMapper;
using BOs.Model;
using Repositories.IRepository;
using Services.IService;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Enum;
using Services.RequestAndResponse.Request.TeamResultRequest;
using Services.RequestAndResponse.Response.TeamResultResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class TeamResultInGameService : ITeamResultInGameService
    {
        private readonly ITeamResultInGameRepository _teamResultRepository;
        private readonly IMapper _mapper;

        public TeamResultInGameService(ITeamResultInGameRepository teamResultRepository, IMapper mapper)
        {
            _teamResultRepository = teamResultRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<TeamResultResponse>> GetTeamResultByIdAsync(int teamResultId)
        {
            try
            {
                var teamResult = await _teamResultRepository.GetByIdAsync(teamResultId);
                if (teamResult == null)
                {
                    return new BaseResponse<TeamResultResponse>("TeamResultInGame not found", StatusCodeEnum.NotFound_404, null);
                }

                var response = _mapper.Map<TeamResultResponse>(teamResult);
                return new BaseResponse<TeamResultResponse>("Successfully retrieved TeamResultInGame", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<TeamResultResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<IEnumerable<TeamResultResponse>>> GetTeamResultsBySessionIdAsync(int sessionId)
        {
            try
            {
                var teamResults = await _teamResultRepository.GetBySessionIdAsync(sessionId);
                if (teamResults == null || !teamResults.Any())
                {
                    return new BaseResponse<IEnumerable<TeamResultResponse>>("No TeamResultInGame records found for this session", StatusCodeEnum.NotFound_404, null);
                }

                var response = _mapper.Map<IEnumerable<TeamResultResponse>>(teamResults);
                return new BaseResponse<IEnumerable<TeamResultResponse>>("Successfully retrieved TeamResultInGame records", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<TeamResultResponse>>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<IEnumerable<TeamResultResponse>>> GetTeamResultsByTeamIdAsync(int teamId)
        {
            try
            {
                var teamResults = await _teamResultRepository.GetByTeamIdAsync(teamId);
                if (teamResults == null || !teamResults.Any())
                {
                    return new BaseResponse<IEnumerable<TeamResultResponse>>("No TeamResultInGame records found for this team", StatusCodeEnum.NotFound_404, null);
                }

                var response = _mapper.Map<IEnumerable<TeamResultResponse>>(teamResults);
                return new BaseResponse<IEnumerable<TeamResultResponse>>("Successfully retrieved TeamResultInGame records", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<TeamResultResponse>>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<TeamResultResponse>> CreateTeamResultAsync(CreateTeamResultRequest request)
        {
            try
            {
                // Ánh xạ từ request sang entity TeamResultInGame
                var teamResult = _mapper.Map<TeamResultInGame>(request);

                // Thêm vào cơ sở dữ liệu
                var createdTeamResult = await _teamResultRepository.AddAsync(teamResult);

                // Ánh xạ từ entity sang response
                var response = _mapper.Map<TeamResultResponse>(createdTeamResult);

                return new BaseResponse<TeamResultResponse>("TeamResultInGame created successfully", StatusCodeEnum.Created_201, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<TeamResultResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<TeamResultResponse>> UpdateTeamResultAsync(UpdateTeamResultRequest request)
        {
            try
            {
                // Lấy TeamResultInGame theo ID
                var teamResult = await _teamResultRepository.GetByIdAsync(request.TeamResultInGameId);
                if (teamResult == null)
                {
                    return new BaseResponse<TeamResultResponse>("TeamResultInGame not found", StatusCodeEnum.NotFound_404, null);
                }

                // Cập nhật thông tin
                teamResult.QuestionInGameId = request.QuestionInGameId;
                teamResult.SessionId = request.SessionId;
                teamResult.TeamId = request.TeamId;
                teamResult.Score = request.Score;

                // Cập nhật vào cơ sở dữ liệu
                var updatedTeamResult = await _teamResultRepository.UpdateAsync(teamResult);

                // Ánh xạ sang response
                var response = _mapper.Map<TeamResultResponse>(updatedTeamResult);

                return new BaseResponse<TeamResultResponse>("TeamResultInGame updated successfully", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<TeamResultResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<string>> DeleteTeamResultAsync(int teamResultId)
        {
            try
            {
                var teamResult = await _teamResultRepository.GetByIdAsync(teamResultId);
                if (teamResult == null)
                {
                    return new BaseResponse<string>("TeamResultInGame not found", StatusCodeEnum.NotFound_404, null);
                }

                await _teamResultRepository.DeleteAsync(teamResult);
                return new BaseResponse<string>("TeamResultInGame deleted successfully", StatusCodeEnum.OK_200, "Deleted successfully");
            }
            catch (Exception ex)
            {
                return new BaseResponse<string>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<IEnumerable<TeamRankingResponse>>> GetTeamRankingsBySessionIdAsync(int sessionId)
        {
            try
            {
                var rankings = await _teamResultRepository.GetBySessionIdAsync(sessionId);

                var grouped = rankings
                    .GroupBy(tr => tr.Team)
                    .Select(g => new
                    {
                        TeamId = g.Key.TeamId,
                        TeamName = g.Key.Name, // đảm bảo entity Team có property Name
                        TotalScore = g.Sum(tr => tr.Score)
                    })
                    .OrderByDescending(g => g.TotalScore)
                    .ToList();

                var response = grouped.Select((r, index) => new TeamRankingResponse
                {
                    TeamId = r.TeamId,
                    TeamName = r.TeamName,
                    TotalScore = r.TotalScore,
                    Rank = index + 1
                });

                return new BaseResponse<IEnumerable<TeamRankingResponse>>("Team rankings retrieved successfully", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<TeamRankingResponse>>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

    }
}
