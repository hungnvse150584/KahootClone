using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs.Model
{
    public class QuestionInGame
    {
        [Key]
        public int QuestionInGameId { get; set; }
        public int SessionId { get; set; }
        public int QuestionId { get; set; }
        public int OrderIndex { get; set; }
        public DateTime CreatedTime { get; set; } // Thêm CreatedTime
        public int TotalMembers { get; set; } // Thêm TotalMembers

        // Quan hệ
        [ForeignKey("SessionId")]
        public GameSession Session { get; set; }

        [ForeignKey("QuestionId")]
        public Question Question { get; set; }

        public ICollection<Response> Responses { get; set; } = new List<Response>();
        public ICollection<TeamResultInGame> TeamResults { get; set; } = new List<TeamResultInGame>();
    }
}
