using Microsoft.AspNetCore.Mvc;
using Services.IService;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Enum;
using Services.RequestAndResponse.Request.QuestionInGameRequest;
using Services.RequestAndResponse.Response.QuestionInGameResponse;
using Services.RequestAndResponse.Response.ResponseResponses;

namespace Kahoot.Controllers
{
    [Route("api/questions-in-game")]
    [ApiController]
    public class QuestionInGameController : ControllerBase
    {
        private readonly IQuestionInGameService _questionInGameService;

        public QuestionInGameController(IQuestionInGameService questionInGameService)
        {
            _questionInGameService = questionInGameService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<QuestionInGameResponse>>> CreateQuestionInGame([FromBody] CreateQuestionInGameRequest request)
        {
            if (request == null || !ModelState.IsValid)
            {
                return BadRequest(new BaseResponse<QuestionInGameResponse>("Invalid request data.", StatusCodeEnum.BadRequest_400, null));
            }

            var result = await _questionInGameService.CreateQuestionInGameAsync(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("{questionInGameId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<QuestionInGameResponse>>> UpdateQuestionInGame(int questionInGameId, [FromBody] UpdateQuestionInGameRequest request)
        {
            if (questionInGameId <= 0 || request == null || !ModelState.IsValid)
            {
                return BadRequest(new BaseResponse<QuestionInGameResponse>("Invalid request.", StatusCodeEnum.BadRequest_400, null));
            }

            request.QuestionInGameId = questionInGameId;

            var result = await _questionInGameService.UpdateQuestionInGameAsync(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{questionInGameId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<QuestionInGameResponse>>> GetQuestionInGameById(int questionInGameId)
        {
            if (questionInGameId <= 0)
            {
                return BadRequest(new BaseResponse<QuestionInGameResponse>("Invalid QuestionInGame ID.", StatusCodeEnum.BadRequest_400, null));
            }

            var result = await _questionInGameService.GetQuestionInGameByIdAsync(questionInGameId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("/api/game-sessions/{sessionId}/questions-in-game")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<IEnumerable<QuestionInGameResponse>>>> GetQuestionsInGameBySessionId(int sessionId)
        {
            if (sessionId <= 0)
            {
                return BadRequest(new BaseResponse<IEnumerable<QuestionInGameResponse>>("Invalid Session ID.", StatusCodeEnum.BadRequest_400, null));
            }

            var result = await _questionInGameService.GetQuestionsInGameBySessionIdAsync(sessionId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{questionInGameId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<string>>> DeleteQuestionInGame(int questionInGameId)
        {
            if (questionInGameId <= 0)
            {
                return BadRequest(new BaseResponse<string>("Invalid QuestionInGame ID.", StatusCodeEnum.BadRequest_400, null));
            }

            var result = await _questionInGameService.DeleteQuestionInGameAsync(questionInGameId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{questionInGameId}/responses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<IEnumerable<ResponseResponse>>>> GetResponsesByQuestionInGameId(int questionInGameId)
        {
            if (questionInGameId <= 0)
            {
                return BadRequest(new BaseResponse<IEnumerable<ResponseResponse>>("Invalid QuestionInGame ID.", StatusCodeEnum.BadRequest_400, null));
            }

            var result = await _questionInGameService.GetResponsesByQuestionInGameIdAsync(questionInGameId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("/api/game-sessions/{sessionId}/players/{playerId}/last-response")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<ResponseResponse>>> GetLastResponseByPlayerIdAndSessionId(int playerId, int sessionId)
        {
            if (playerId <= 0 || sessionId <= 0)
            {
                return BadRequest(new BaseResponse<ResponseResponse>("Invalid Player ID or Session ID.", StatusCodeEnum.BadRequest_400, null));
            }

            var result = await _questionInGameService.GetLastResponseByPlayerIdAndSessionIdAsync(playerId, sessionId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
