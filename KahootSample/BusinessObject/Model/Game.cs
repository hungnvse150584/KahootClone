using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    public class Game
    {
        [Key]
        public int GameId { get; set; }

        [Required, MaxLength(10)]
        public string GamePin { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public string Status { get; set; } = "Pending"; // Pending, Active, Ended

        [Required]
        public string Mode { get; set; } = "Classic"; // Classic, Team

        // Quan hệ
        [ForeignKey("CreatedBy")]
        public User CreatedByUser { get; set; }

        public GameConfig GameConfig { get; set; }
        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<PlayerSession> PlayerSessions { get; set; } = new List<PlayerSession>();
        public ICollection<Team> Teams { get; set; } = new List<Team>();
        public ICollection<GameResult> GameResults { get; set; } = new List<GameResult>();
    }
}
