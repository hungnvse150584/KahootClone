using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RequestAndResponse.Response.SummaryResponse
{
    public class SummaryReportResponse
    {
        public int SessionId { get; set; }
        public DateTime StartedAt { get; set; } // Changed from CreatedTime to StartedAt
        public int QuizId { get; set; }
        public string QuizTitle { get; set; }   
        //public string? Duration { get; set; }
        public int TotalPlayers { get; set; }
        public double AverageScore { get; set; }
        public double HighestPlayerScore { get; set; }
        public List<QuestionSummary> Questions { get; set; } = new List<QuestionSummary>();
        public List<TeamSummary> Teams { get; set; } = new List<TeamSummary>();
        public List<PlayerSummary> Players { get; set; } = new List<PlayerSummary>();
        public string HighestScoringTeam { get; set; }
        public string SessionStatus { get; set; } // Added to reflect GameSession.Status
    }
}
