using AutoMapper;
using BOs.Model;
using Repositories.IRepository;
using Services.IService;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Enum;
using Services.RequestAndResponse.Request.UserRequest;
using Services.RequestAndResponse.Response.UserResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<LoginResponse>> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userRepository.GetByUsernameAsync(request.Username);
                if (user == null || user.Password != request.Password)
                {
                    return new BaseResponse<LoginResponse>("Invalid username or password", StatusCodeEnum.Unauthorized_401, null);
                }

                //// Optional role-based restriction example
                //if (user.Role != UserRoles.Admin && user.Role != UserRoles.Host)
                //{
                //    return new BaseResponse<LoginResponse>("Access denied for this role", StatusCodeEnum.Forbidden_403, null);
                //}

                var userResponse = _mapper.Map<UserResponse>(user);
                var loginResponse = new LoginResponse
                {
                    Token = "MockJWTToken123456", // Replace with real JWT logic later
                    User = userResponse
                };

                return new BaseResponse<LoginResponse>("Login successful", StatusCodeEnum.OK_200, loginResponse);
            }
            catch (Exception ex)
            {
                return new BaseResponse<LoginResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<UserResponse>> CreateUserAsync(CreateUserRequest request)
        {
            try
            {
                var existingEmailUser = await _userRepository.GetByEmailAsync(request.Email);
                if (existingEmailUser != null)
                {
                    return new BaseResponse<UserResponse>("Email is already taken.", StatusCodeEnum.BadRequest_400, null);
                }

                var existingUsernameUser = await _userRepository.GetByUsernameAsync(request.Username);
                if (existingUsernameUser != null)
                {
                    return new BaseResponse<UserResponse>("Username is already taken.", StatusCodeEnum.BadRequest_400, null);
                }

                var user = _mapper.Map<User>(request);
                user.Role = request.Role ?? UserRoles.Host;
                var createdUser = await _userRepository.AddAsync(user);
                var response = _mapper.Map<UserResponse>(createdUser);

                return new BaseResponse<UserResponse>("User created successfully", StatusCodeEnum.Created_201, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<UserResponse>> UpdateUserAsync(UpdateUserRequest request)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(request.UserId);
                if (user == null)
                {
                    return new BaseResponse<UserResponse>("User not found", StatusCodeEnum.NotFound_404, null);
                }

                if (!string.IsNullOrWhiteSpace(request.Username) && request.Username != user.Username)
                {
                    var existingUsernameUser = await _userRepository.GetByUsernameAsync(request.Username);
                    if (existingUsernameUser != null && existingUsernameUser.UserId != user.UserId)
                    {
                        return new BaseResponse<UserResponse>("Username is already taken.", StatusCodeEnum.BadRequest_400, null);
                    }
                    user.Username = request.Username;
                }

                // Email check and update
                if (!string.IsNullOrWhiteSpace(request.Email) && request.Email != user.Email)
                {
                    var existingEmailUser = await _userRepository.GetByEmailAsync(request.Email);
                    if (existingEmailUser != null && existingEmailUser.UserId != user.UserId)
                    {
                        return new BaseResponse<UserResponse>("Email is already taken.", StatusCodeEnum.BadRequest_400, null);
                    }
                    user.Email = request.Email;
                }

                // Password update
                if (!string.IsNullOrWhiteSpace(request.Password))
                {
                    user.Password = request.Password;
                }

                // Role update (optional)
                if (request.Role.HasValue)
                {
                    user.Role = request.Role.Value;
                }

                var updatedUser = await _userRepository.UpdateAsync(user);
                var response = _mapper.Map<UserResponse>(updatedUser);

                return new BaseResponse<UserResponse>("User updated successfully", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<UserResponse>> GetUserByIdAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return new BaseResponse<UserResponse>("User not found", StatusCodeEnum.NotFound_404, null);
                }

                var response = _mapper.Map<UserResponse>(user);
                return new BaseResponse<UserResponse>("Successfully retrieved user", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<UserResponse>> GetUserByUsernameAsync(string username)
        {
            try
            {
                var user = await _userRepository.GetByUsernameAsync(username);
                if (user == null)
                {
                    return new BaseResponse<UserResponse>("User not found", StatusCodeEnum.NotFound_404, null);
                }

                var response = _mapper.Map<UserResponse>(user);
                return new BaseResponse<UserResponse>("Successfully retrieved user", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<UserResponse>> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(email);
                if (user == null)
                {
                    return new BaseResponse<UserResponse>("User not found", StatusCodeEnum.NotFound_404, null);
                }

                var response = _mapper.Map<UserResponse>(user);
                return new BaseResponse<UserResponse>("Successfully retrieved user", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<IEnumerable<UserResponse>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userRepository.GetAllAsync();
                if (users == null || !users.Any())
                {
                    return new BaseResponse<IEnumerable<UserResponse>>("No users found", StatusCodeEnum.NotFound_404, null);
                }

                var response = _mapper.Map<IEnumerable<UserResponse>>(users);
                return new BaseResponse<IEnumerable<UserResponse>>("Successfully retrieved users", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<UserResponse>>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<string>> DeleteUserAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return new BaseResponse<string>("User not found", StatusCodeEnum.NotFound_404, null);
                }

                await _userRepository.DeleteAsync(user);
                return new BaseResponse<string>("User deleted successfully", StatusCodeEnum.OK_200, "Deleted successfully");
            }
            catch (Exception ex)
            {
                return new BaseResponse<string>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }
    }
}
