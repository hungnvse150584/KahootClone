using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    public class GameResult
    {
        [Key]
        public int GameResultId { get; set; }

        public int GameId { get; set; }

        public int? PlayerSessionId { get; set; }

        public int? TeamId { get; set; }

        public int FinalScore { get; set; }

        public int Rank { get; set; }

        // Quan hệ
        [ForeignKey("GameId")]
        public Game Game { get; set; }

        [ForeignKey("PlayerSessionId")]
        public PlayerSession PlayerSession { get; set; }

        [ForeignKey("TeamId")]
        public Team Team { get; set; }
    }
}
