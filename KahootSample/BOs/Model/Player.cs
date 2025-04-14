using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace BOs.Model
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }

        public int SessionId { get; set; }

        public int? UserId { get; set; }

        [Required, MaxLength(50)]
        public string Nickname { get; set; }

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("SessionId")]
        public GameSession Session { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public ICollection<Response> Responses { get; set; } = new List<Response>();
        public ICollection<Score> Scores { get; set; } = new List<Score>();
        public ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
    }
}
