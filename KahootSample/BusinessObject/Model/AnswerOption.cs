using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    public class AnswerOption
    {
        [Key]
        public int AnswerOptionId { get; set; }

        public int QuestionId { get; set; }

        [Required, MaxLength(200)]
        public string Content { get; set; }

        public bool IsCorrect { get; set; }

        // Quan hệ
        [ForeignKey("QuestionId")]
        public Question Question { get; set; }

        public ICollection<PlayerAnswer> PlayerAnswers { get; set; } = new List<PlayerAnswer>();
    }
}
