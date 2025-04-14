using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs.Model
{
    public class Quiz
    {
        [Key]
        public int QuizId { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(10)]
        //[Index(IsUnique = true)]
        public string Pin { get; set; }

        [MaxLength(200)]
        public string QrCode { get; set; }

        // Quan hệ
        [ForeignKey("CreatedBy")]
        public User CreatedByUser { get; set; }

        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<GameSession> GameSessions { get; set; } = new List<GameSession>();
    }
}
