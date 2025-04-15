using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RequestAndResponse.Request.GameSessionRequest
{
    public class UpdateGameSessionRequest
    {
        [Required(ErrorMessage = "QuizId is required")]
        public int QuizId { get; set; }

        public DateTime? StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; }

        public bool EnableSpeedBonus { get; set; }
        public bool EnableStreak { get; set; }

        public string GameMode { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "MaxPlayers must be greater than 0")]
        public int MaxPlayers { get; set; }

        public bool AutoAdvance { get; set; }
        public bool ShowLeaderboard { get; set; }
    }
}

