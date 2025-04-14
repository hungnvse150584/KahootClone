using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs.Model
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }

        public int SessionId { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        public double TotalScore { get; set; }

        // Quan hệ
        [ForeignKey("SessionId")]
        public GameSession Session { get; set; }

        public ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
    }
}
