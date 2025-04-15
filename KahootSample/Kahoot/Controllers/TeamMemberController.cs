using Microsoft.AspNetCore.Mvc;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Enum;
using Services.RequestAndResponse.Request.TeamMemberRequest;
using Services.RequestAndResponse.Response;
using Services.RequestAndResponse.Response.TeamMemberResponses;

namespace Kahoot.Controllers
{
    [Route("api/teammember")]
    [ApiController]
    public class TeamMemberController : ControllerBase
    {
        private readonly ITeamMemberService _teamMemberService;

        public TeamMemberController(ITeamMemberService teamMemberService)
        {
            _teamMemberService = teamMemberService;
        }

        [HttpPost]
        [Route("CreateTeamMember")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<TeamMemberResponse>>> CreateTeamMember([FromBody] CreateTeamMemberRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new BaseResponse<TeamMemberResponse>("Request body cannot be null!", StatusCodeEnum.BadRequest_400, null));
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new BaseResponse<TeamMemberResponse>("Invalid request data!", StatusCodeEnum.BadRequest_400, null));
                }

                var result = await _teamMemberService.CreateTeamMemberAsync(request);
                return StatusCode((int)result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<TeamMemberResponse>($"Something went wrong! Error: {ex.Message}", StatusCodeEnum.InternalServerError_500, null));
            }
        }

        [HttpGet]
        [Route("GetTeamMember/{teamMemberId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<TeamMemberResponse>>> GetTeamMemberById(int teamMemberId)
        {
            try
            {
                var result = await _teamMemberService.GetTeamMemberByIdAsync(teamMemberId);
                return StatusCode((int)result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<TeamMemberResponse>($"Something went wrong! Error: {ex.Message}", StatusCodeEnum.InternalServerError_500, null));
            }
        }

        [HttpGet]
        [Route("GetAllTeamMembers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<IEnumerable<TeamMemberResponse>>>> GetAllTeamMembers()
        {
            try
            {
                var result = await _teamMemberService.GetAllTeamMembersAsync();
                return StatusCode((int)result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<IEnumerable<TeamMemberResponse>>($"Something went wrong! Error: {ex.Message}", StatusCodeEnum.InternalServerError_500, null));
            }
        }

        [HttpPut]
        [Route("UpdateTeamMember/{teamMemberId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<TeamMemberResponse>>> UpdateTeamMember(int teamMemberId, [FromBody] UpdateTeamMemberRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new BaseResponse<TeamMemberResponse>("Request body cannot be null!", StatusCodeEnum.BadRequest_400, null));
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new BaseResponse<TeamMemberResponse>("Invalid request data!", StatusCodeEnum.BadRequest_400, null));
                }

                var result = await _teamMemberService.UpdateTeamMemberAsync(teamMemberId, request);
                return StatusCode((int)result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<TeamMemberResponse>($"Something went wrong! Error: {ex.Message}", StatusCodeEnum.InternalServerError_500, null));
            }
        }

        [HttpDelete]
        [Route("DeleteTeamMember/{teamMemberId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<string>>> DeleteTeamMember(int teamMemberId)
        {
            try
            {
                var result = await _teamMemberService.DeleteTeamMemberAsync(teamMemberId);
                return StatusCode((int)result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<string>($"Something went wrong! Error: {ex.Message}", StatusCodeEnum.InternalServerError_500, null));
            }
        }

        [HttpGet]
        [Route("GetTeamMembersByTeam/{teamId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<IEnumerable<TeamMemberResponse>>>> GetTeamMembersByTeamId(int teamId)
        {
            try
            {
                var result = await _teamMemberService.GetTeamMembersByTeamIdAsync(teamId);
                return StatusCode((int)result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<IEnumerable<TeamMemberResponse>>($"Something went wrong! Error: {ex.Message}", StatusCodeEnum.InternalServerError_500, null));
            }
        }

        [HttpGet]
        [Route("GetTeamMembersByPlayer/{playerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<IEnumerable<TeamMemberResponse>>>> GetTeamMembersByPlayerId(int playerId)
        {
            try
            {
                var result = await _teamMemberService.GetTeamMembersByPlayerIdAsync(playerId);
                return StatusCode((int)result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<IEnumerable<TeamMemberResponse>>($"Something went wrong! Error: {ex.Message}", StatusCodeEnum.InternalServerError_500, null));
            }
        }
    }
}
