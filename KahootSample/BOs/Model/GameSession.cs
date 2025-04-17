using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOs.Model
{
    public class GameSession
    {
        [Key]
        public int SessionId { get; set; }
        public int QuizId { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        [MaxLength(10)]
        public string Pin { get; set; }
        [Required, MaxLength(50)]
        public string Status { get; set; } = "Pending";
        public bool EnableSpeedBonus { get; set; } = true;
        public bool EnableStreak { get; set; } = true;
        [MaxLength(50)]
        public string GameMode { get; set; } = "Classic";
        public int MaxPlayers { get; set; } = 50;
        public bool AutoAdvance { get; set; } = true;
        public bool ShowLeaderboard { get; set; } = true;
        public bool LoadingInGame { get; set; }
        public int? FinalScore { get; set; }

        [ForeignKey("QuizId")]
        public Quiz Quiz { get; set; }
        public ICollection<QuestionInGame> QuestionsInGame { get; set; } = new List<QuestionInGame>();
        public ICollection<Player> Players { get; set; } = new List<Player>();
        public ICollection<Team> Teams { get; set; } = new List<Team>();
        public ICollection<Score> Scores { get; set; } = new List<Score>();
        public ICollection<TeamResultInGame> TeamResultsInGame { get; set; } = new List<TeamResultInGame>();
    }
}