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
        public int CreatedBy { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Status { get; set; } // Thêm Status

        // Quan hệ
        [ForeignKey("CreatedBy")]
        public User CreatedByUser { get; set; }

        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<GameSession> GameSessions { get; set; } = new List<GameSession>();
    }
}
