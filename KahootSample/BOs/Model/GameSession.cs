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
        public DateTime StartedAt { get; set; }
        public string Status { get; set; }
        public string Pin { get; set; }
        public bool EnableSpeedBonus { get; set; }
        public bool EnableStreak { get; set; }
        public string GameMode { get; set; }
        public int MaxPlayers { get; set; }
        public bool AutoAdvance { get; set; }
        public bool ShowLeaderboard { get; set; }
        public bool LoadingInGame { get; set; }

        // Quan hệ
        [ForeignKey("QuizId")]
        public Quiz Quiz { get; set; }

        public ICollection<QuestionInGame> QuestionsInGame { get; set; } = new List<QuestionInGame>();
        public ICollection<Player> Players { get; set; } = new List<Player>();
        public ICollection<Team> Teams { get; set; } = new List<Team>();
        public ICollection<TeamResultInGame> TeamResultsInGame { get; set; } = new List<TeamResultInGame>();
    }
}