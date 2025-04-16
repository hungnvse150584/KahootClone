using Microsoft.AspNetCore.Mvc;
using Services.IService;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Enum;
using Services.RequestAndResponse.Request.TeamRequest;
using Services.RequestAndResponse.Response;
using Services.RequestAndResponse.Response.TeamMemberResponses;
using Services.RequestAndResponse.Response.TeamResponse;
using System.Threading.Tasks;

namespace Kahoot.Controllers
{
    [Route("api/team")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpPost]
        [Route("CreateTeam")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<TeamResponse>>> CreateTeam([FromBody] CreateTeamRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new BaseResponse<TeamResponse>("Request body cannot be null!", StatusCodeEnum.BadRequest_400, null));
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new BaseResponse<TeamResponse>("Invalid request data!", StatusCodeEnum.BadRequest_400, null));
                }

                var result = await _teamService.CreateTeamAsync(request);
                return StatusCode((int)result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<TeamResponse>($"Something went wrong! Error: {ex.Message}", StatusCodeEnum.InternalServerError_500, null));
            }
        }

        [HttpPut]
        [Route("UpdateTeam/{teamId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<TeamResponse>>> UpdateTeam(int teamId, [FromBody] UpdateTeamRequest request)
        {
            if (teamId <= 0)
            {
                return BadRequest(new BaseResponse<TeamResponse>("Invalid Team ID.", StatusCodeEnum.BadRequest_400, null));
            }

            if (request == null)
            {
                return BadRequest(new BaseResponse<TeamResponse>("Request body cannot be null.", StatusCodeEnum.BadRequest_400, null));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponse<TeamResponse>("Invalid request data.", StatusCodeEnum.BadRequest_400, null));
            }

            var result = await _teamService.UpdateTeamAsync(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        [Route("GetTeamById/{teamId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<TeamResponse>>> GetTeamById(int teamId)
        {
            if (teamId <= 0)
            {
                return BadRequest(new BaseResponse<TeamResponse>("Please provide a valid Team ID.", StatusCodeEnum.BadRequest_400, null));
            }

            var result = await _teamService.GetTeamByIdAsync(teamId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        [Route("GetTeamsBySessionId/{sessionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<IEnumerable<TeamResponse>>>> GetTeamsBySessionId(int sessionId)
        {
            if (sessionId <= 0)
            {
                return BadRequest(new BaseResponse<IEnumerable<TeamResponse>>("Please provide a valid Session ID.", StatusCodeEnum.BadRequest_400, null));
            }

            var result = await _teamService.GetTeamsBySessionIdAsync(sessionId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete]
        [Route("DeleteTeam/{teamId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<string>>> DeleteTeam(int teamId)
        {
            if (teamId <= 0)
            {
                return BadRequest(new BaseResponse<string>("Please provide a valid Team ID.", StatusCodeEnum.BadRequest_400, null));
            }

            var result = await _teamService.DeleteTeamAsync(teamId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        [Route("AddTeamMember/{teamId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<TeamMemberResponse>>> AddTeamMember(int teamId, [FromQuery] int playerId)
        {
            if (teamId <= 0)
            {
                return BadRequest(new BaseResponse<TeamMemberResponse>("Please provide a valid Team ID.", StatusCodeEnum.BadRequest_400, null));
            }

            if (playerId <= 0)
            {
                return BadRequest(new BaseResponse<TeamMemberResponse>("Please provide a valid Player ID.", StatusCodeEnum.BadRequest_400, null));
            }

            var result = await _teamService.AddTeamMemberAsync(teamId, playerId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        [Route("GetTeamMembersByTeamId/{teamId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<IEnumerable<TeamMemberResponse>>>> GetTeamMembersByTeamId(int teamId)
        {
            if (teamId <= 0)
            {
                return BadRequest(new BaseResponse<IEnumerable<TeamMemberResponse>>("Please provide a valid Team ID.", StatusCodeEnum.BadRequest_400, null));
            }

            var result = await _teamService.GetTeamMembersByTeamIdAsync(teamId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}