using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    public class PlayerAnswer
    {
        [Key]
        public int PlayerAnswerId { get; set; }

        public int PlayerSessionId { get; set; }

        public int QuestionId { get; set; }

        public int AnswerOptionId { get; set; }

        public bool IsCorrect { get; set; }

        public float ResponseTime { get; set; } 

        public int Score { get; set; }

        public int Streak { get; set; } = 0;

        // Quan hệ
        [ForeignKey("PlayerSessionId")]
        public PlayerSession PlayerSession { get; set; }

        [ForeignKey("QuestionId")]
        public Question Question { get; set; }

        [ForeignKey("AnswerOptionId")]
        public AnswerOption AnswerOption { get; set; }
    }
}
