using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs.Model
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }

        public int QuizId { get; set; }

        [Required]
        public string Text { get; set; }

        public int TimeLimit { get; set; }

        public int Order { get; set; }

        // Quan hệ
        [ForeignKey("QuizId")]
        public Quiz Quiz { get; set; }

        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
        public ICollection<Response> Responses { get; set; } = new List<Response>();
    }
}
