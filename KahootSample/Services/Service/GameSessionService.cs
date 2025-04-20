using AutoMapper;
using BOs.Model;
using Repositories.IRepository;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Enum;
using Services.RequestAndResponse.Request.GameSessionRequest;
using Services.RequestAndResponse.Response.GameSessionResponses;

namespace Services.Service
{
    public class GameSessionService : IGameSessionService
    {
        private readonly IGameSessionRepository _gameSessionRepository;
        private readonly IMapper _mapper;

        public GameSessionService(IGameSessionRepository gameSessionRepository, IMapper mapper)
        {
            _gameSessionRepository = gameSessionRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GameSessionResponse>> CreateGameSessionAsync(CreateGameSessionRequest request)
        {
            try
            {
                var gameSession = _mapper.Map<GameSession>(request);
                var createdSession = await _gameSessionRepository.AddAsync(gameSession);
                var response = _mapper.Map<GameSessionResponse>(createdSession);
                return new BaseResponse<GameSessionResponse>("GameSession created successfully", StatusCodeEnum.Created_201, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<GameSessionResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<GameSessionResponse>> UpdateGameSessionAsync(int sessionId, UpdateGameSessionRequest request)
        {
            try
            {
                // Kiểm tra xem GameSession có tồn tại không
                var gameSession = await _gameSessionRepository.GetByIdAsync(sessionId);
                if (gameSession == null)
                {
                    return new BaseResponse<GameSessionResponse>($"GameSession with id {sessionId} not found", StatusCodeEnum.NotFound_404, null);
                }

                // Cập nhật thông tin
                gameSession.QuizId = request.QuizId;
                gameSession.StartedAt = (DateTime)request.StartedAt;
                gameSession.Status = request.Status;
                gameSession.Pin = request.Pin;
                gameSession.EnableSpeedBonus = request.EnableSpeedBonus;
                gameSession.EnableStreak = request.EnableStreak;
                gameSession.GameMode = request.GameMode;
                gameSession.MaxPlayers = request.MaxPlayers;
                gameSession.AutoAdvance = request.AutoAdvance;
                gameSession.ShowLeaderboard = request.ShowLeaderboard;

                // Cập nhật vào cơ sở dữ liệu
                var updatedSession = await _gameSessionRepository.UpdateAsync(gameSession);

                // Ánh xạ sang response
                var response = _mapper.Map<GameSessionResponse>(updatedSession);

                return new BaseResponse<GameSessionResponse>("GameSession updated successfully", StatusCodeEnum.OK_200, response);
            }
            catch (ArgumentNullException ex)
            {
                return new BaseResponse<GameSessionResponse>(ex.Message, StatusCodeEnum.NotFound_404, null);
            }
            catch (Exception ex)
            {
                return new BaseResponse<GameSessionResponse>($"An error occurred while updating GameSession: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<GameSessionResponse>> GetGameSessionByIdAsync(int sessionId)
        {
            try
            {
                var gameSession = await _gameSessionRepository.GetByIdAsync(sessionId);
                if (gameSession == null)
                {
                    return new BaseResponse<GameSessionResponse>("GameSession not found", StatusCodeEnum.NotFound_404, null);
                }

                var response = _mapper.Map<GameSessionResponse>(gameSession);
                return new BaseResponse<GameSessionResponse>("Successfully retrieved GameSession", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<GameSessionResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<IEnumerable<GameSessionResponse>>> GetGameSessionsByQuizIdAsync(int quizId)
        {
            try
            {
                var gameSessions = await _gameSessionRepository.GetGameSessionsByQuizIdAsync(quizId);
                if (gameSessions == null || !gameSessions.Any())
                {
                    return new BaseResponse<IEnumerable<GameSessionResponse>>("No GameSessions found for this quiz", StatusCodeEnum.NotFound_404, null);
                }

                var response = _mapper.Map<IEnumerable<GameSessionResponse>>(gameSessions);
                return new BaseResponse<IEnumerable<GameSessionResponse>>("Successfully retrieved GameSessions", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<GameSessionResponse>>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<string>> DeleteGameSessionAsync(int sessionId)
        {
            try
            {
                var gameSession = await _gameSessionRepository.GetByIdAsync(sessionId);
                if (gameSession == null)
                {
                    return new BaseResponse<string>("GameSession not found", StatusCodeEnum.NotFound_404, null);
                }

                await _gameSessionRepository.DeleteAsync(gameSession);
                return new BaseResponse<string>("GameSession deleted successfully", StatusCodeEnum.OK_200, "Deleted successfully");
            }
            catch (Exception ex)
            {
                return new BaseResponse<string>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<IEnumerable<GameSessionResponse>>> GetAllGameSessionsAsync()
        {
            try
            {
                var gameSessions = await _gameSessionRepository.GetAllAsync();
                if (gameSessions == null || !gameSessions.Any())
                {
                    return new BaseResponse<IEnumerable<GameSessionResponse>>("No GameSessions found", StatusCodeEnum.NotFound_404, null);
                }

                var response = _mapper.Map<IEnumerable<GameSessionResponse>>(gameSessions);
                return new BaseResponse<IEnumerable<GameSessionResponse>>("Successfully retrieved all GameSessions", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<GameSessionResponse>>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }
    }
}