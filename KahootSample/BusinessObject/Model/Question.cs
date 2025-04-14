using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }

        public int GameId { get; set; }

        [Required, MaxLength(500)]
        public string Content { get; set; }

        [Required]
        public string QuestionType { get; set; } = "MCQ"; // MCQ, TrueFalse

        public int TimeLimit { get; set; } = 20; // Giây

        public int Order { get; set; }

        // Quan hệ
        [ForeignKey("GameId")]
        public Game Game { get; set; }

        public ICollection<AnswerOption> AnswerOptions { get; set; } = new List<AnswerOption>();
        public ICollection<PlayerAnswer> PlayerAnswers { get; set; } = new List<PlayerAnswer>();
    }
}
