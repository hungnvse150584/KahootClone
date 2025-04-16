﻿using Microsoft.AspNetCore.Mvc;
using Services.IService;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Enum;
using Services.RequestAndResponse.Request;
using Services.RequestAndResponse.Response.QuizResponses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kahoot.Controllers
{
    [Route("api/quiz")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpPost]
        [Route("CreateQuiz")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<QuizResponse>>> CreateQuiz([FromBody] CreateQuizRequest request)
        {
            if (request == null)
            {
                return BadRequest(new BaseResponse<QuizResponse>("Request body cannot be null!", StatusCodeEnum.BadRequest_400, null));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponse<QuizResponse>("Invalid request data!", StatusCodeEnum.BadRequest_400, null));
            }

            var result = await _quizService.CreateQuizAsync(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        [Route("UpdateQuiz/{quizId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<QuizResponse>>> UpdateQuiz(int quizId, [FromBody] UpdateQuizRequest request)
        {
            if (quizId <= 0 || request == null)
            {
                return BadRequest(new BaseResponse<QuizResponse>("Invalid Quiz ID or request body.", StatusCodeEnum.BadRequest_400, null));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponse<QuizResponse>("Invalid request data.", StatusCodeEnum.BadRequest_400, null));
            }

            request.QuizId = quizId;
            var result = await _quizService.UpdateQuizAsync(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        [Route("GetQuizById/{quizId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<QuizResponse>>> GetQuizById(int quizId)
        {
            if (quizId <= 0)
            {
                return BadRequest(new BaseResponse<QuizResponse>("Please provide a valid Quiz ID.", StatusCodeEnum.BadRequest_400, null));
            }

            var result = await _quizService.GetQuizByIdAsync(quizId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        [Route("GetQuizzesByUserId/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<IEnumerable<QuizResponse>>>> GetQuizzesByUserId(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest(new BaseResponse<IEnumerable<QuizResponse>>("Please provide a valid User ID.", StatusCodeEnum.BadRequest_400, null));
            }

            var result = await _quizService.GetQuizzesByUserIdAsync(userId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        [Route("SearchByTitle")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<IEnumerable<QuizResponse>>>> SearchByTitle([FromQuery] string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return BadRequest(new BaseResponse<IEnumerable<QuizResponse>>("Title is required for searching.", StatusCodeEnum.BadRequest_400, null));
            }

            var result = await _quizService.SearchQuizzesByTitleAsync(title);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete]
        [Route("DeleteQuiz/{quizId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<string>>> DeleteQuiz(int quizId)
        {
            if (quizId <= 0)
            {
                return BadRequest(new BaseResponse<string>("Please provide a valid Quiz ID.", StatusCodeEnum.BadRequest_400, null));
            }

            var result = await _quizService.DeleteQuizAsync(quizId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
