using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs.Model
{
    public class Answer
    {
        [Key]
        public int AnswerId { get; set; }

        public int QuestionId { get; set; }

        [Required, MaxLength(200)]
        public string Text { get; set; }

        public bool IsCorrect { get; set; }

        // Quan hệ
        [ForeignKey("QuestionId")]
        public Question Question { get; set; }

        public ICollection<Response> Responses { get; set; } = new List<Response>();
    }
}
