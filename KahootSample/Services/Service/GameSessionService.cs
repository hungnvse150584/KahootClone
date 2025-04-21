using AutoMapper;
using BOs.Model;
using Repositories.IRepository;
using Services.IService;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Enum;
using Services.RequestAndResponse.Request.GameSessionRequest;
using Services.RequestAndResponse.Response.GameSessionResponses;
using Services.RequestAndResponse.Response.PlayerResponse;
using Services.RequestAndResponse.Response.TeamResponse;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
                // Generate a random Pin if not provided
                if (string.IsNullOrEmpty(request.Pin))
                {
                    request.Pin = GenerateRandomPin();
                }

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
                var gameSession = await _gameSessionRepository.GetByIdAsync(sessionId);
                if (gameSession == null)
                {
                    return new BaseResponse<GameSessionResponse>($"GameSession with id {sessionId} not found", StatusCodeEnum.NotFound_404, null);
                }

                gameSession.QuizId = request.QuizId;
                gameSession.StartedAt = request.StartedAt ?? gameSession.StartedAt;
                gameSession.Status = request.Status;
                gameSession.Pin = request.Pin;
                gameSession.EnableSpeedBonus = request.EnableSpeedBonus;
                gameSession.EnableStreak = request.EnableStreak;
                gameSession.GameMode = request.GameMode;
                gameSession.MaxPlayers = request.MaxPlayers;
                gameSession.AutoAdvance = request.AutoAdvance;
                gameSession.ShowLeaderboard = request.ShowLeaderboard;
                gameSession.LoadingInGame = false; // Reset loading state

                var updatedSession = await _gameSessionRepository.UpdateAsync(gameSession);
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

        public async Task<BaseResponse<GameSessionResponse>> GetGameSessionByPinAsync(string pin)
        {
            try
            {
                if (string.IsNullOrEmpty(pin))
                {
                    return new BaseResponse<GameSessionResponse>("Pin cannot be empty", StatusCodeEnum.BadRequest_400, null);
                }

                var gameSession = await _gameSessionRepository.GetGameSessionByPinAsync(pin);
                if (gameSession == null)
                {
                    return new BaseResponse<GameSessionResponse>("GameSession not found", StatusCodeEnum.NotFound_404, null);
                }

                var response = _mapper.Map<GameSessionResponse>(gameSession);
                return new BaseResponse<GameSessionResponse>("Successfully retrieved GameSession", StatusCodeEnum.OK_200, response);
            }
            catch (ArgumentNullException ex)
            {
                return new BaseResponse<GameSessionResponse>(ex.Message, StatusCodeEnum.NotFound_404, null);
            }
            catch (Exception ex)
            {
                return new BaseResponse<GameSessionResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<IEnumerable<PlayerResponse>>> GetPlayersInSessionAsync(int sessionId)
        {
            try
            {
                var players = await _gameSessionRepository.GetPlayersInSessionAsync(sessionId);
                if (players == null || !players.Any())
                {
                    return new BaseResponse<IEnumerable<PlayerResponse>>("No players found in this session", StatusCodeEnum.NotFound_404, null);
                }

                var response = _mapper.Map<IEnumerable<PlayerResponse>>(players);
                return new BaseResponse<IEnumerable<PlayerResponse>>("Successfully retrieved players", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<PlayerResponse>>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<IEnumerable<TeamResponse>>> GetTeamsInSessionAsync(int sessionId)
        {
            try
            {
                var teams = await _gameSessionRepository.GetTeamsInSessionAsync(sessionId);
                if (teams == null || !teams.Any())
                {
                    return new BaseResponse<IEnumerable<TeamResponse>>("No teams found in this session", StatusCodeEnum.NotFound_404, null);
                }

                var response = _mapper.Map<IEnumerable<TeamResponse>>(teams);
                return new BaseResponse<IEnumerable<TeamResponse>>("Successfully retrieved teams", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<TeamResponse>>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<string>> EndGameSessionAsync(int sessionId)
        {
            try
            {
                var gameSession = await _gameSessionRepository.GetByIdAsync(sessionId);
                if (gameSession == null)
                {
                    return new BaseResponse<string>("GameSession not found", StatusCodeEnum.NotFound_404, null);
                }

                if (gameSession.Status == "Ended")
                {
                    return new BaseResponse<string>("GameSession already ended", StatusCodeEnum.BadRequest_400, null);
                }

                gameSession.Status = "Ended";
                await _gameSessionRepository.UpdateAsync(gameSession);
                return new BaseResponse<string>("GameSession ended successfully", StatusCodeEnum.OK_200, "Ended");
            }
            catch (Exception ex)
            {
                return new BaseResponse<string>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        private string GenerateRandomPin()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString(); // Tạo mã PIN 6 chữ số
        }
    }
}