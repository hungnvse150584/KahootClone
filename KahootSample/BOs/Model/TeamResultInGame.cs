using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOs.Model
{
    public class TeamResultInGame
    {
        [Key]
        public int TeamResultInGameId { get; set; }
        public int QuestionInGameId { get; set; }
        public int SessionId { get; set; }
        public int TeamId { get; set; }
        public int Score { get; set; }

        // Quan hệ
        [ForeignKey("QuestionInGameId")]
        public QuestionInGame QuestionInGame { get; set; }

        [ForeignKey("SessionId")]
        public GameSession Session { get; set; }

        [ForeignKey("TeamId")]
        public Team Team { get; set; }
    }
}