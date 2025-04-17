using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOs.Model
{
    public class Response
    {
        [Key]
        public int ResponseId { get; set; }

        public int PlayerId { get; set; }
        public int QuestionInGameId { get; set; }
        public int SelectedOption { get; set; }
        public int ResponseTime { get; set; }
        public int Score { get; set; }
        public int Streak { get; set; }
        public int TotalMembers { get; set; }

        // Quan hệ
        [ForeignKey("PlayerId")]
        public Player Player { get; set; }

        [ForeignKey("QuestionInGameId")]
        public QuestionInGame QuestionInGame { get; set; }
    }
}