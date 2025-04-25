using System;
using System.ComponentModel.DataAnnotations;
using BOs.Model;

namespace Services.RequestAndResponse.Request.UserRequest
{
    public class UpdateUserRequest
    {
        [Key]
        public int UserId { get; set; }

        [MaxLength(50)]
        public string Username { get; set; }

        [MaxLength(256)]
        public string Password { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        public UserRoles? Role { get; set; } // Add this line
    }
}
