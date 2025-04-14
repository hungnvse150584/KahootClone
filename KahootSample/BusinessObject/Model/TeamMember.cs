using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    public class TeamMember
    {
        [Key]
        public int TeamMemberId { get; set; }

        public int TeamId { get; set; }

        public int PlayerSessionId { get; set; }

        public int Score { get; set; } = 0;

        // Quan hệ
        [ForeignKey("TeamId")]
        public Team Team { get; set; }

        [ForeignKey("PlayerSessionId")]
        public PlayerSession PlayerSession { get; set; }
    }
}
