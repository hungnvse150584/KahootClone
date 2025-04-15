using Microsoft.AspNetCore.Mvc;
using Services.IService;
using System.Threading.Tasks;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Enum;
using Services.RequestAndResponse.Request.GameSessionRequest;
using Services.RequestAndResponse.Response.GameSessionResponses;

namespace Kahoot.Controllers
{
    [Route("api/gamesession")]
    [ApiController]
    public class GameSessionController : ControllerBase
    {
        private readonly IGameSessionService _gameSessionService;

        public GameSessionController(IGameSessionService gameSessionService)
        {
            _gameSessionService = gameSessionService;
        }

        [HttpPost]
        [Route("CreateGameSession")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<GameSessionResponse>>> CreateGameSession([FromBody] CreateGameSessionRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new BaseResponse<GameSessionResponse>("Request body cannot be null!", StatusCodeEnum.BadRequest_400, null));
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new BaseResponse<GameSessionResponse>("Invalid request data!", StatusCodeEnum.BadRequest_400, null));
                }

                var result = await _gameSessionService.CreateGameSessionAsync(request);
                return StatusCode((int)result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<GameSessionResponse>($"Something went wrong! Error: {ex.Message}", StatusCodeEnum.InternalServerError_500, null));
            }
        }

        [HttpGet]
        [Route("GetGameSession/{sessionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<GameSessionResponse>>> GetGameSessionById(int sessionId)
        {
            try
            {
                var result = await _gameSessionService.GetGameSessionByIdAsync(sessionId);
                return StatusCode((int)result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<GameSessionResponse>($"Something went wrong! Error: {ex.Message}", StatusCodeEnum.InternalServerError_500, null));
            }
        }

        [HttpGet]
        [Route("GetAllGameSessions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<IEnumerable<GameSessionResponse>>>> GetAllGameSessions()
        {
            try
            {
                var result = await _gameSessionService.GetAllGameSessionsAsync();
                return StatusCode((int)result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<IEnumerable<GameSessionResponse>>($"Something went wrong! Error: {ex.Message}", StatusCodeEnum.InternalServerError_500, null));
            }
        }

        [HttpPut]
        [Route("UpdateGameSession/{sessionId}")] // Thêm sessionId vào route
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<GameSessionResponse>>> UpdateGameSession(int sessionId, [FromBody] UpdateGameSessionRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new BaseResponse<GameSessionResponse>("Request body cannot be null!", StatusCodeEnum.BadRequest_400, null));
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new BaseResponse<GameSessionResponse>("Invalid request data!", StatusCodeEnum.BadRequest_400, null));
                }

                var result = await _gameSessionService.UpdateGameSessionAsync(sessionId, request);
                return StatusCode((int)result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<GameSessionResponse>($"Something went wrong! Error: {ex.Message}", StatusCodeEnum.InternalServerError_500, null));
            }
        }

        [HttpDelete]
        [Route("DeleteGameSession/{sessionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<string>>> DeleteGameSession(int sessionId)
        {
            try
            {
                var result = await _gameSessionService.DeleteGameSessionAsync(sessionId);
                return StatusCode((int)result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<string>($"Something went wrong! Error: {ex.Message}", StatusCodeEnum.InternalServerError_500, null));
            }
        }

        [HttpGet]
        [Route("GetGameSessionsByQuiz/{quizId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<IEnumerable<GameSessionResponse>>>> GetGameSessionsByQuizId(int quizId)
        {
            try
            {
                var result = await _gameSessionService.GetGameSessionsByQuizIdAsync(quizId);
                return StatusCode((int)result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<IEnumerable<GameSessionResponse>>($"Something went wrong! Error: {ex.Message}", StatusCodeEnum.InternalServerError_500, null));
            }
        }
    }
}