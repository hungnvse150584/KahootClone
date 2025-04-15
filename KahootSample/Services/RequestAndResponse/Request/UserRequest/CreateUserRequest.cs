using System;
using System.ComponentModel.DataAnnotations;

namespace Services.RequestAndResponse.Request.UserRequest
{
    public class CreateUserRequest
    {
        [Required, MaxLength(50)]
        public string Username { get; set; }

        [Required, MaxLength(256)]
        public string Password { get; set; }

        [Required, MaxLength(100)]
        public string Email { get; set; }
    }
}
