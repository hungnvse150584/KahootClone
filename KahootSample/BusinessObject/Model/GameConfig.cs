using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    public class GameConfig
    {
        [Key]
        public int GameConfigId { get; set; }

        public int GameId { get; set; }

        public bool EnableStreak { get; set; } = true;

        public bool EnableSpeedBonus { get; set; } = true;

        public int MaxPlayers { get; set; } = 50;

        public int MaxTeams { get; set; } = 10;

        // Quan hệ
        [ForeignKey("GameId")]
        public Game Game { get; set; }
    }
}
