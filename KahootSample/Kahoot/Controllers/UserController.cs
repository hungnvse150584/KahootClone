using Microsoft.AspNetCore.Mvc;
using Services.IService;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Enum;
using Services.RequestAndResponse.Request.UserRequest;
using Services.RequestAndResponse.Response.UserResponse;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kahoot.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<BaseResponse<LoginResponse>>> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return Unauthorized(new BaseResponse<LoginResponse>("Invalid credentials", StatusCodeEnum.Unauthorized_401, null));

            var result = await _userService.LoginAsync(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<UserResponse>>> CreateUser([FromBody] CreateUserRequest request)
        {
            if (request == null)
                return BadRequest(new BaseResponse<UserResponse>("Request body cannot be null!", StatusCodeEnum.BadRequest_400, null));

            if (!ModelState.IsValid)
                return BadRequest(new BaseResponse<UserResponse>("Invalid request data!", StatusCodeEnum.BadRequest_400, null));

            var result = await _userService.CreateUserAsync(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<UserResponse>>> UpdateUser(int userId, [FromBody] UpdateUserRequest request)
        {
            if (userId <= 0)
                return BadRequest(new BaseResponse<UserResponse>("Invalid User ID.", StatusCodeEnum.BadRequest_400, null));

            if (request == null)
                return BadRequest(new BaseResponse<UserResponse>("Request body cannot be null.", StatusCodeEnum.BadRequest_400, null));

            if (!ModelState.IsValid)
                return BadRequest(new BaseResponse<UserResponse>("Invalid request data.", StatusCodeEnum.BadRequest_400, null));

            request.UserId = userId;
            var result = await _userService.UpdateUserAsync(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<UserResponse>>> GetUserById(int userId)
        {
            if (userId <= 0)
                return BadRequest(new BaseResponse<UserResponse>("Please provide a valid User ID.", StatusCodeEnum.BadRequest_400, null));

            var result = await _userService.GetUserByIdAsync(userId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("by-username")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<UserResponse>>> GetUserByUsername([FromQuery] string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest(new BaseResponse<UserResponse>("Username must not be empty.", StatusCodeEnum.BadRequest_400, null));

            var result = await _userService.GetUserByUsernameAsync(username);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("by-email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<UserResponse>>> GetUserByEmail([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest(new BaseResponse<UserResponse>("Email must not be empty.", StatusCodeEnum.BadRequest_400, null));

            var result = await _userService.GetUserByEmailAsync(email);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<IEnumerable<UserResponse>>>> GetAllUsers()
        {
            var result = await _userService.GetAllUsersAsync();
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<string>>> DeleteUser(int userId)
        {
            if (userId <= 0)
                return BadRequest(new BaseResponse<string>("Please provide a valid User ID.", StatusCodeEnum.BadRequest_400, null));

            var result = await _userService.DeleteUserAsync(userId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
