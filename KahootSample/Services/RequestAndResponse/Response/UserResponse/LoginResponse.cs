using Services.RequestAndResponse.Response.UserResponse;

namespace Services.RequestAndResponse.Response.UserResponse
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public UserResponse User { get; set; }
    }
}