using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOs.Model
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }
        public int SessionId { get; set; }
        public string Name { get; set; }
        public double TotalScore { get; set; }

        // Quan hệ
        [ForeignKey("SessionId")]
        public GameSession Session { get; set; }
        public ICollection<Player> Players { get; set; } = new List<Player>();
        public ICollection<TeamResultInGame> TeamResults { get; set; } = new List<TeamResultInGame>();
    }
}