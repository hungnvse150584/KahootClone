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
        private readonly IQuestionService _questionService;

        public GameSessionController(IGameSessionService gameSessionService, IQuestionService questionService)
        {
            _gameSessionService = gameSessionService;
            _questionService = questionService;
        }

        [HttpPost]
        [Route("Start/{sessionId}")]
        public async Task<ActionResult<BaseResponse<string>>> StartGameSession(int sessionId)
        {
            if (sessionId <= 0)
            {
                return BadRequest(new BaseResponse<string>("Invalid Session ID", StatusCodeEnum.BadRequest_400, null));
            }

            var session = await _gameSessionService.GetGameSessionByIdAsync(sessionId);
            if (session == null)
            {
                return NotFound(new BaseResponse<string>("GameSession not found", StatusCodeEnum.NotFound_404, null));
            }

            // Kiểm tra quyền của host (có thể thêm logic xác thực tại đây)

            return Ok(new BaseResponse<string>("GameSession started", StatusCodeEnum.OK_200, "Started"));
        }

        [HttpPost]
        [Route("NextQuestion/{sessionId}/{questionId}")]
        public async Task<ActionResult<BaseResponse<string>>> NextQuestion(int sessionId, int questionId)
        {
            if (sessionId <= 0 || questionId <= 0)
            {
                return BadRequest(new BaseResponse<string>("Invalid Session ID or Question ID", StatusCodeEnum.BadRequest_400, null));
            }

            var session = await _gameSessionService.GetGameSessionByIdAsync(sessionId);
            if (session == null)
            {
                return NotFound(new BaseResponse<string>("GameSession not found", StatusCodeEnum.NotFound_404, null));
            }

            var question = await _questionService.GetQuestionByIdAsync(questionId);
            if (question.StatusCode != StatusCodeEnum.OK_200 || question.Data == null)
            {
                return NotFound(new BaseResponse<string>("Question not found", StatusCodeEnum.NotFound_404, null));
            }

            // Gửi câu hỏi qua SignalR Hub (sẽ được xử lý trong Hub)
            return Ok(new BaseResponse<string>("Question sent", StatusCodeEnum.OK_200, "Sent"));
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
        [Route("UpdateGameSession/{sessionId}")]
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