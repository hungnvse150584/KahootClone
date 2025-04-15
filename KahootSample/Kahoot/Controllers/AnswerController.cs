using Microsoft.AspNetCore.Mvc;
using Services.IService;
using Services.RequestAndResponse.Request.AnswerRequest;

namespace Kahoot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerService _answerService;

        public AnswerController(IAnswerService answerService)
        {
            _answerService = answerService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAnswer([FromBody] CreateAnswerRequest request)
        {
            var result = await _answerService.CreateAnswerAsync(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAnswer([FromBody] UpdateAnswerRequest request)
        {
            var result = await _answerService.UpdateAnswerAsync(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnswerById(int id)
        {
            var result = await _answerService.GetAnswerByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("by-question/{questionId}")]
        public async Task<IActionResult> GetAnswersByQuestionId(int questionId)
        {
            var result = await _answerService.GetAnswersByQuestionIdAsync(questionId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnswer(int id)
        {
            var result = await _answerService.DeleteAnswerAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
