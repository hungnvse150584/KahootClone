using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BOs.Model;

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
        [JsonIgnore]
        public UserRoles? Role { get; set; } // Default value

    }
}
