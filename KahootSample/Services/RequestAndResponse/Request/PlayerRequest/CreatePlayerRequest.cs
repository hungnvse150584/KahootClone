using System;
using System.ComponentModel.DataAnnotations;

namespace Services.RequestAndResponse.PlayerRequest
{
    public class CreatePlayerRequest
    {
        [Required]
        public int? SessionId { get; set; }

        [Required, MaxLength(50)]
        public string Nickname { get; set; }

        [Required]
        public DateTime JoinedAt { get; set; }

        public int? Score { get; set; } = 0; // Default to 0, as set in GameSessionHub

        public int? Ranking { get; set; } = 0; // Default to 0, as set in GameSessionHub

        public int? UserId { get; set; } // Optional, matches Player entity

        public int? TeamId { get; set; } // Optional, matches Player entity
    }
}
