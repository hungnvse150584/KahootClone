using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BOs.Model;
using Repositories.IRepository;
using Services.IService;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Enum;
using Services.RequestAndResponse.Request.ScoreRequest;
using Services.RequestAndResponse.Response.ScoreResponse;

namespace Services.Service
{
    public class ScoreService : IScoreService
    {
        private readonly IScoreRepository _scoreRepository;
        private readonly IMapper _mapper;

        public ScoreService(IScoreRepository scoreRepository, IMapper mapper)
        {
            _scoreRepository = scoreRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<ScoreResponse>> CreateScoreAsync(CreateScoreRequest request)
        {
            try
            {
                var score = _mapper.Map<Score>(request);
                await _scoreRepository.AddAsync(score);
                var response = _mapper.Map<ScoreResponse>(score);

                return new BaseResponse<ScoreResponse>("Score created successfully", StatusCodeEnum.Created_201, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<ScoreResponse>($"Error: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<ScoreResponse>> UpdateScoreAsync(UpdateScoreRequest request)
        {
            try
            {
                var score = await _scoreRepository.GetByIdAsync(request.ScoreId);
                if (score == null)
                    return new BaseResponse<ScoreResponse>("Score not found", StatusCodeEnum.NotFound_404, null);

                score.TotalPoints = request.TotalPoints;
                await _scoreRepository.UpdateAsync(score);

                var response = _mapper.Map<ScoreResponse>(score);
                return new BaseResponse<ScoreResponse>("Score updated successfully", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<ScoreResponse>($"Error: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<string>> DeleteScoreAsync(int scoreId)
        {
            try
            {
                var score = await _scoreRepository.GetByIdAsync(scoreId);
                if (score == null)
                    return new BaseResponse<string>("Score not found", StatusCodeEnum.NotFound_404, null);

                await _scoreRepository.DeleteAsync(score);
                return new BaseResponse<string>("Score deleted successfully", StatusCodeEnum.OK_200, "Deleted");
            }
            catch (Exception ex)
            {
                return new BaseResponse<string>($"Error: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<ScoreResponse>> GetScoreByIdAsync(int id)
        {
            try
            {
                var score = await _scoreRepository.GetByIdAsync(id);
                if (score == null)
                    return new BaseResponse<ScoreResponse>("Score not found", StatusCodeEnum.NotFound_404, null);

                var response = _mapper.Map<ScoreResponse>(score);
                return new BaseResponse<ScoreResponse>("Success", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<ScoreResponse>($"Error: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }
        public async Task<BaseResponse<IEnumerable<ScoreResponse>>> GetScoresByPlayerIdAsync(int playerId)
        {
            try
            {
                var scores = await _scoreRepository.GetScoresByPlayerIdAsync(playerId);
                var response = _mapper.Map<IEnumerable<ScoreResponse>>(scores);
                return new BaseResponse<IEnumerable<ScoreResponse>>("Scores retrieved successfully", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<ScoreResponse>>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }
        public async Task<BaseResponse<IEnumerable<ScoreResponse>>> GetScoresBySessionIdAsync(int sessionId)
        {
            try
            {
                var scores = await _scoreRepository.GetBySessionIdAsync(sessionId);
                var response = _mapper.Map<IEnumerable<ScoreResponse>>(scores);

                return new BaseResponse<IEnumerable<ScoreResponse>>("Success", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<ScoreResponse>>($"Error: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }
        public async Task<BaseResponse<IEnumerable<ScoreResponse>>> GetTopScoresAsync(int topN)
        {
            try
            {
                var scores = await _scoreRepository.GetTopScoresAsync(topN);
                var response = _mapper.Map<IEnumerable<ScoreResponse>>(scores);
                return new BaseResponse<IEnumerable<ScoreResponse>>("Top scores retrieved successfully", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<ScoreResponse>>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<int>> GetTotalPointsByPlayerAsync(int playerId)
        {
            try
            {
                var total = await _scoreRepository.GetTotalPointsByPlayerAsync(playerId);
                return new BaseResponse<int>("Total points retrieved successfully", StatusCodeEnum.OK_200, total);
            }
            catch (Exception ex)
            {
                return new BaseResponse<int>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, 0);
            }
        }
    }

}
