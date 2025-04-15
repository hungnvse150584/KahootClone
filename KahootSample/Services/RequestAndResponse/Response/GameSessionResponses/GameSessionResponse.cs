using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RequestAndResponse.Response.GameSessionResponses
{
    public class GameSessionResponse
    {
        public int SessionId { get; set; }
        public int QuizId { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public string Status { get; set; }
        public bool EnableSpeedBonus { get; set; }
        public bool EnableStreak { get; set; }
        public string GameMode { get; set; }
        public int MaxPlayers { get; set; }
        public bool AutoAdvance { get; set; }
        public bool ShowLeaderboard { get; set; }
    }
}
