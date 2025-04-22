using Microsoft.AspNetCore.Mvc;
using Services.IService;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Enum;
using Services.RequestAndResponse.Request.GameSessionRequest;
using Services.RequestAndResponse.Response.GameSessionResponses;
using Services.RequestAndResponse.Response.PlayerResponse;
using Services.RequestAndResponse.Response.TeamResponse;
using System.Threading.Tasks;

namespace Kahoot.Controllers
{
    [Route("api/gamesession")]
    [ApiController]
    public class GameSessionController : ControllerBase
    {
        private readonly IGameSessionService _gameSessionService;
        private readonly IQuestionService _questionService;

        public GameSessionController(
            IGameSessionService gameSessionService,
            IQuestionService questionService)
        {
            _gameSessionService = gameSessionService;
            _questionService = questionService;
        }

        [HttpPost("Start/{sessionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseResponse<string>>> StartGameSession(int sessionId)
        {
            if (sessionId <= 0)
            {
                return BadRequest(new BaseResponse<string>("Invalid Session ID", StatusCodeEnum.BadRequest_400, null));
            }

            var sessionResponse = await _gameSessionService.GetGameSessionByIdAsync(sessionId);
            if (sessionResponse.StatusCode != StatusCodeEnum.OK_200 || sessionResponse.Data == null)
            {
                return NotFound(new BaseResponse<string>("GameSession not found", StatusCodeEnum.NotFound_404, null));
            }

            if (sessionResponse.Data.Status == "Started")
            {
                return BadRequest(new BaseResponse<string>("GameSession already started", StatusCodeEnum.BadRequest_400, null));
            }

            var updateRequest = new UpdateGameSessionRequest
            {
                QuizId = sessionResponse.Data.QuizId,
                StartedAt = sessionResponse.Data.StartedAt ?? DateTime.UtcNow,
                Status = "Started",
                Pin = sessionResponse.Data.Pin,
                EnableSpeedBonus = sessionResponse.Data.EnableSpeedBonus,
                EnableStreak = sessionResponse.Data.EnableStreak,
                GameMode = sessionResponse.Data.GameMode,
                MaxPlayers = sessionResponse.Data.MaxPlayers,
                AutoAdvance = sessionResponse.Data.AutoAdvance,
                ShowLeaderboard = sessionResponse.Data.ShowLeaderboard
            };

            var updateResult = await _gameSessionService.UpdateGameSessionAsync(sessionId, updateRequest);
            if (updateResult.StatusCode != StatusCodeEnum.OK_200)
            {
                return StatusCode((int)updateResult.StatusCode, updateResult);
            }

            return Ok(new BaseResponse<string>("GameSession started", StatusCodeEnum.OK_200, "Started"));
        }

        [HttpPost("NextQuestion/{sessionId}/{questionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseResponse<string>>> NextQuestion(int sessionId, int questionId)
        {
            if (sessionId <= 0 || questionId <= 0)
            {
                return BadRequest(new BaseResponse<string>("Invalid Session ID or Question ID", StatusCodeEnum.BadRequest_400, null));
            }

            var sessionResponse = await _gameSessionService.GetGameSessionByIdAsync(sessionId);
            if (sessionResponse.StatusCode != StatusCodeEnum.OK_200 || sessionResponse.Data == null)
            {
                return NotFound(new BaseResponse<string>("GameSession not found", StatusCodeEnum.NotFound_404, null));
            }

            var question = await _questionService.GetQuestionByIdAsync(questionId);
            if (question.StatusCode != StatusCodeEnum.OK_200 || question.Data == null)
            {
                return NotFound(new BaseResponse<string>("Question not found", StatusCodeEnum.NotFound_404, null));
            }

            return Ok(new BaseResponse<string>("Question sent", StatusCodeEnum.OK_200, "Sent"));
        }

        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<GameSessionResponse>>> CreateGameSession([FromBody] CreateGameSessionRequest request)
        {
            if (request == null)
            {
                return BadRequest(new BaseResponse<GameSessionResponse>("Request body cannot be null", StatusCodeEnum.BadRequest_400, null));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponse<GameSessionResponse>("Invalid request data", StatusCodeEnum.BadRequest_400, null));
            }

            var result = await _gameSessionService.CreateGameSessionAsync(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("GetById/{sessionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<GameSessionResponse>>> GetGameSessionById(int sessionId)
        {
            var result = await _gameSessionService.GetGameSessionByIdAsync(sessionId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<IEnumerable<GameSessionResponse>>>> GetAllGameSessions()
        {
            var result = await _gameSessionService.GetAllGameSessionsAsync();
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("Update/{sessionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<GameSessionResponse>>> UpdateGameSession(int sessionId, [FromBody] UpdateGameSessionRequest request)
        {
            if (request == null)
            {
                return BadRequest(new BaseResponse<GameSessionResponse>("Request body cannot be null", StatusCodeEnum.BadRequest_400, null));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponse<GameSessionResponse>("Invalid request data", StatusCodeEnum.BadRequest_400, null));
            }

            var result = await _gameSessionService.UpdateGameSessionAsync(sessionId, request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("Delete/{sessionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<string>>> DeleteGameSession(int sessionId)
        {
            var sessionResponse = await _gameSessionService.GetGameSessionByIdAsync(sessionId);
            if (sessionResponse.StatusCode != StatusCodeEnum.OK_200 || sessionResponse.Data == null)
            {
                return NotFound(new BaseResponse<string>("GameSession not found", StatusCodeEnum.NotFound_404, null));
            }

            var result = await _gameSessionService.DeleteGameSessionAsync(sessionId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("GetByQuizId/{quizId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<IEnumerable<GameSessionResponse>>>> GetGameSessionsByQuizId(int quizId)
        {
            var result = await _gameSessionService.GetGameSessionsByQuizIdAsync(quizId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("GetGameSessionWithPin/{pin}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<GameSessionResponse>>> GetGameSessionWithPin(string pin)
        {
            var result = await _gameSessionService.GetGameSessionByPinAsync(pin);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("GetPlayersInSession/{sessionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<IEnumerable<PlayerResponse>>>> GetPlayersInSession(int sessionId)
        {
            var result = await _gameSessionService.GetPlayersInSessionAsync(sessionId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("GetTeamsInSession/{sessionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<IEnumerable<TeamResponse>>>> GetTeamsInSession(int sessionId)
        {
            var result = await _gameSessionService.GetTeamsInSessionAsync(sessionId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("End/{sessionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<string>>> EndGameSession(int sessionId)
        {
            var result = await _gameSessionService.EndGameSessionAsync(sessionId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}