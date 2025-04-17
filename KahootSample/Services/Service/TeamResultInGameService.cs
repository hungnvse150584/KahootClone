using AutoMapper;
using BOs.Model;
using Repositories.IRepository;
using Services.IService;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Enum;
using Services.RequestAndResponse.Response.TeamResultInGameResponse;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<BaseResponse<TeamResultInGameResponse>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _teamResultRepository.GetByIdAsync(id);
                if (result == null)
                {
                    return new BaseResponse<TeamResultInGameResponse>("Team result not found", StatusCodeEnum.NotFound_404, null);
                }

                var response = _mapper.Map<TeamResultInGameResponse>(result);
                return new BaseResponse<TeamResultInGameResponse>("Successfully retrieved team result", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<TeamResultInGameResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<IEnumerable<TeamResultInGameResponse>>> GetBySessionIdAsync(int sessionId)
        {
            try
            {
                var results = await _teamResultRepository.GetBySessionIdAsync(sessionId);
                if (results == null || !results.Any())
                {
                    return new BaseResponse<IEnumerable<TeamResultInGameResponse>>("No team results found for this session", StatusCodeEnum.NotFound_404, null);
                }

                var response = _mapper.Map<IEnumerable<TeamResultInGameResponse>>(results);
                return new BaseResponse<IEnumerable<TeamResultInGameResponse>>("Successfully retrieved results", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<TeamResultInGameResponse>>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<IEnumerable<TeamResultInGameResponse>>> GetByTeamIdAsync(int teamId)
        {
            try
            {
                var results = await _teamResultRepository.GetByTeamIdAsync(teamId);
                if (results == null || !results.Any())
                {
                    return new BaseResponse<IEnumerable<TeamResultInGameResponse>>("No team results found for this team", StatusCodeEnum.NotFound_404, null);
                }

                var response = _mapper.Map<IEnumerable<TeamResultInGameResponse>>(results);
                return new BaseResponse<IEnumerable<TeamResultInGameResponse>>("Successfully retrieved results", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<TeamResultInGameResponse>>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<int>> GetTotalTeamScoreInSessionAsync(int teamId, int sessionId)
        {
            try
            {
                var totalScore = await _teamResultRepository.GetTotalTeamScoreInSessionAsync(teamId, sessionId);
                return new BaseResponse<int>("Successfully retrieved total team score", StatusCodeEnum.OK_200, totalScore);
            }
            catch (Exception ex)
            {
                return new BaseResponse<int>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, 0);
            }
        }
    }
}
