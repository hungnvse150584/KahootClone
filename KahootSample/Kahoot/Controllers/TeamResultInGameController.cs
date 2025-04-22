using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.IService;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Request.TeamResultRequest;
using Services.RequestAndResponse.Response.TeamResultResponses;

namespace Kahoot.Controllers
{
    [Route("api/team-result")]
    [ApiController]
    public class TeamResultInGameController : ControllerBase
    {
        private readonly ITeamResultInGameService _teamResultService;

        public TeamResultInGameController(ITeamResultInGameService teamResultService)
        {
            _teamResultService = teamResultService;
        }

      
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<BaseResponse<TeamResultResponse>>> GetTeamResultById(int id)
        {
            var result = await _teamResultService.GetTeamResultByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

       
        [HttpGet("GetBySessionId/{sessionId}")]
        public async Task<ActionResult<BaseResponse<IEnumerable<TeamResultResponse>>>> GetTeamResultsBySessionId(int sessionId)
        {
            var result = await _teamResultService.GetTeamResultsBySessionIdAsync(sessionId);
            return StatusCode((int)result.StatusCode, result);
        }

      
        [HttpGet("GetByTeamId/{teamId}")]
        public async Task<ActionResult<BaseResponse<IEnumerable<TeamResultResponse>>>> GetTeamResultsByTeamId(int teamId)
        {
            var result = await _teamResultService.GetTeamResultsByTeamIdAsync(teamId);
            return StatusCode((int)result.StatusCode, result);
        }

      
        [HttpPost("Create")]
        public async Task<ActionResult<BaseResponse<TeamResultResponse>>> CreateTeamResult([FromBody] CreateTeamResultRequest request)
        {
            var result = await _teamResultService.CreateTeamResultAsync(request);
            return StatusCode((int)result.StatusCode, result);
        }

      
        [HttpPut("Update")]
        public async Task<ActionResult<BaseResponse<TeamResultResponse>>> UpdateTeamResult([FromBody] UpdateTeamResultRequest request)
        {
            var result = await _teamResultService.UpdateTeamResultAsync(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<BaseResponse<string>>> DeleteTeamResult(int id)
        {
            var result = await _teamResultService.DeleteTeamResultAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
