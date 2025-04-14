using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }

        public int GameId { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        public int TotalScore { get; set; } = 0;

        // Quan hệ
        [ForeignKey("GameId")]
        public Game Game { get; set; }

        public ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
        public ICollection<GameResult> GameResults { get; set; } = new List<GameResult>();
    }
}
