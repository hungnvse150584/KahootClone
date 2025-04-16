using BOs.Model;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Request.UserRequest;
using Services.RequestAndResponse.Response.UserResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface IUserService
    {
        Task<BaseResponse<UserResponse>> CreateUserAsync(CreateUserRequest request);
        Task<BaseResponse<UserResponse>> UpdateUserAsync(UpdateUserRequest request);
        Task<BaseResponse<UserResponse>> GetUserByIdAsync(int userId);
        Task<BaseResponse<UserResponse>> GetUserByUsernameAsync(string username);
        Task<BaseResponse<UserResponse>> GetUserByEmailAsync(string email);
        Task<BaseResponse<IEnumerable<UserResponse>>> GetAllUsersAsync();
        Task<BaseResponse<string>> DeleteUserAsync(int userId);
    }
}
