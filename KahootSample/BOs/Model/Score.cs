using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs.Model
{
    public class Score
    {
        [Key]
        public int ScoreId { get; set; }

        public int PlayerId { get; set; }

        public int SessionId { get; set; }

        public int TotalPoints { get; set; }

        // Quan hệ
        [ForeignKey("PlayerId")]
        public Player Player { get; set; }

        [ForeignKey("SessionId")]
        public GameSession Session { get; set; }
    }
}
