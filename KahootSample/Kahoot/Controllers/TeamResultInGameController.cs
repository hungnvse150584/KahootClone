using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.IService;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Request.TeamResultRequest;
using Services.RequestAndResponse.Response.TeamResultResponses;

namespace Kahoot.Controllers
{
    [Route("api/team-results")]
    [ApiController]
    public class TeamResultInGameController : ControllerBase
    {
        private readonly ITeamResultInGameService _teamResultService;

        public TeamResultInGameController(ITeamResultInGameService teamResultService)
        {
            _teamResultService = teamResultService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<TeamResultResponse>>> GetTeamResultById(int id)
        {
            var result = await _teamResultService.GetTeamResultByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("session/{sessionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<IEnumerable<TeamResultResponse>>>> GetTeamResultsBySessionId(int sessionId)
        {
            var result = await _teamResultService.GetTeamResultsBySessionIdAsync(sessionId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("team/{teamId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<IEnumerable<TeamResultResponse>>>> GetTeamResultsByTeamId(int teamId)
        {
            var result = await _teamResultService.GetTeamResultsByTeamIdAsync(teamId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<TeamResultResponse>>> CreateTeamResult([FromBody] CreateTeamResultRequest request)
        {
            var result = await _teamResultService.CreateTeamResultAsync(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<TeamResultResponse>>> UpdateTeamResult([FromBody] UpdateTeamResultRequest request)
        {
            var result = await _teamResultService.UpdateTeamResultAsync(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<string>>> DeleteTeamResult(int id)
        {
            var result = await _teamResultService.DeleteTeamResultAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
