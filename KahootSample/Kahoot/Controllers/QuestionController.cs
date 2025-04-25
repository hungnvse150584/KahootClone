using Microsoft.AspNetCore.Mvc;
using Services.IService;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Enum;
using Services.RequestAndResponse.Request.QuestionRequest;
using Services.RequestAndResponse.Response.QuestionResponses;
using Services.RequestAndResponse.Response.ResponseResponses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kahoot.Controllers
{
    [Route("api/questions")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<QuestionResponse>>> CreateQuestion([FromBody] CreateQuestionRequest request)
        {
            if (request == null || !ModelState.IsValid)
            {
                return BadRequest(new BaseResponse<QuestionResponse>("Invalid request data.", StatusCodeEnum.BadRequest_400, null));
            }

            var result = await _questionService.CreateQuestionAsync(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("{questionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<QuestionResponse>>> UpdateQuestion(int questionId, [FromBody] UpdateQuestionRequest request)
        {
            if (questionId <= 0 || request == null || !ModelState.IsValid)
            {
                return BadRequest(new BaseResponse<QuestionResponse>("Invalid request.", StatusCodeEnum.BadRequest_400, null));
            }

            request.QuestionId = questionId;

            var result = await _questionService.UpdateQuestionAsync(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<IEnumerable<QuestionResponse>>>> GetAllQuestions()
        {
            var result = await _questionService.GetAllQuestionsAsync();
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{questionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<QuestionResponse>>> GetQuestionById(int questionId)
        {
            if (questionId <= 0)
            {
                return BadRequest(new BaseResponse<QuestionResponse>("Invalid Question ID.", StatusCodeEnum.BadRequest_400, null));
            }

            var result = await _questionService.GetQuestionByIdAsync(questionId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("/quizzes/{quizId}/questions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<IEnumerable<QuestionResponse>>>> GetQuestionsByQuizId(int quizId)
        {
            if (quizId <= 0)
            {
                return BadRequest(new BaseResponse<IEnumerable<QuestionResponse>>("Invalid Quiz ID.", StatusCodeEnum.BadRequest_400, null));
            }

            var result = await _questionService.GetQuestionsByQuizIdAsync(quizId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{questionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<string>>> DeleteQuestion(int questionId)
        {
            if (questionId <= 0)
            {
                return BadRequest(new BaseResponse<string>("Invalid Question ID.", StatusCodeEnum.BadRequest_400, null));
            }

            var result = await _questionService.DeleteQuestionAsync(questionId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("questions-in-game/{questionInGameId}/responses")]
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

            var result = await _questionService.GetResponsesByQuestionInGameIdAsync(questionInGameId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("/players/{playerId}/quizzes/{quizId}/last-responses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<ResponseResponse>>> GetLastResponseByPlayerIdAndQuizId(int playerId, int quizId)
        {
            if (playerId <= 0 || quizId <= 0)
            {
                return BadRequest(new BaseResponse<ResponseResponse>("Invalid Player ID or Quiz ID.", StatusCodeEnum.BadRequest_400, null));
            }

            var result = await _questionService.GetLastResponseByPlayerIdAndQuizIdAsync(playerId, quizId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
