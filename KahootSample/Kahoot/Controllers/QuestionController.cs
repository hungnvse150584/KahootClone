using Microsoft.AspNetCore.Mvc;
using Services.IService;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Enum;
using Services.RequestAndResponse.Request.QuestionRequest;
using Services.RequestAndResponse.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kahoot.Controllers
{
    [Route("api/question")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpPost]
        [Route("CreateQuestion")]
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

        [HttpPut]
        [Route("UpdateQuestion/{questionId}")]
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
        //[HttpGet]
        //[Route("GetQuestions")]
        //[ProducesResponseType(StatusCode)]


        [HttpGet]
        [Route("GetQuestionById/{questionId}")]
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

        [HttpGet]
        [Route("GetQuestionsByQuizId/{quizId}")]
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

        [HttpDelete]
        [Route("DeleteQuestion/{questionId}")]
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
    }
}
