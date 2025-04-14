using System;
using System.ComponentModel.DataAnnotations;

namespace Services.RequestAndResponse.Request
{
    public class UpdateUserRequest
    {
        [Key]
        public int UserId { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; }

        [Required, MaxLength(256)]
        public string Password { get; set; }

        [Required, MaxLength(100)]
        public string Email { get; set; }
    }
}
