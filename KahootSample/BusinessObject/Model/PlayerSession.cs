using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    public class PlayerSession
    {
        [Key]
        public int PlayerSessionId { get; set; }

        public int GameId { get; set; }

        public int? UserId { get; set; }

        [Required, MaxLength(50)]
        public string Nickname { get; set; }

        public DateTime JoinTime { get; set; } = DateTime.UtcNow;

        public int TotalScore { get; set; } = 0;

        // Quan hệ
        [ForeignKey("GameId")]
        public Game Game { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
        public ICollection<PlayerAnswer> PlayerAnswers { get; set; } = new List<PlayerAnswer>();
        public ICollection<GameResult> GameResults { get; set; } = new List<GameResult>();
    }
}
