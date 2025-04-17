using Microsoft.AspNetCore.Mvc;
using Services.IService;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Enum;
using Services.RequestAndResponse.Response.TeamResultInGameResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<TeamResultInGameResponse>>> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new BaseResponse<TeamResultInGameResponse>("Invalid ID", StatusCodeEnum.BadRequest_400, null));
            }

            var result = await _teamResultService.GetByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("GetBySessionId/{sessionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<IEnumerable<TeamResultInGameResponse>>>> GetBySessionId(int sessionId)
        {
            if (sessionId <= 0)
            {
                return BadRequest(new BaseResponse<IEnumerable<TeamResultInGameResponse>>("Invalid session ID", StatusCodeEnum.BadRequest_400, null));
            }

            var result = await _teamResultService.GetBySessionIdAsync(sessionId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("GetByTeamId/{teamId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<IEnumerable<TeamResultInGameResponse>>>> GetByTeamId(int teamId)
        {
            if (teamId <= 0)
            {
                return BadRequest(new BaseResponse<IEnumerable<TeamResultInGameResponse>>("Invalid team ID", StatusCodeEnum.BadRequest_400, null));
            }

            var result = await _teamResultService.GetByTeamIdAsync(teamId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("GetTotalScore")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<int>>> GetTotalScore([FromQuery] int teamId, [FromQuery] int sessionId)
        {
            if (teamId <= 0 || sessionId <= 0)
            {
                return BadRequest(new BaseResponse<int>("Invalid team ID or session ID", StatusCodeEnum.BadRequest_400, 0));
            }

            var result = await _teamResultService.GetTotalTeamScoreInSessionAsync(teamId, sessionId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
