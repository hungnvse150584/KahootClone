using System.ComponentModel.DataAnnotations;

namespace Services.RequestAndResponse.Request.UserRequest
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}