using Microsoft.AspNetCore.Mvc;
using Services.IService;
using Services.RequestAndResponse.Request.QuestionInGameRequest;

namespace Kahoot.Controllers
{
    [ApiController]
    [Route("api/question-in-game")]
    public class QuestionInGameController : ControllerBase
    {
        private readonly IQuestionInGameService _service;

        public QuestionInGameController(IQuestionInGameService service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateQuestionInGameRequest request)
        {
            var result = await _service.CreateAsync(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateQuestionInGameRequest request)
        {
            var result = await _service.UpdateAsync(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("session/{sessionId}")]
        public async Task<IActionResult> GetBySession(int sessionId)
        {
            var result = await _service.GetBySessionIdAsync(sessionId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }

}
