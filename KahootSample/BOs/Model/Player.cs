using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOs.Model
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }
        public int SessionId { get; set; }
        public int? UserId { get; set; }
        public string Nickname { get; set; }
        public DateTime JoinedAt { get; set; }
        public int? TeamId { get; set; }
        public int? Ranking { get; set; }
        public int? Score { get; set; }

        // Quan hệ
        [ForeignKey("SessionId")]
        public GameSession Session { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("TeamId")]
        public Team Team { get; set; }

        public ICollection<Response> Responses { get; set; } = new List<Response>();
     
    }
}