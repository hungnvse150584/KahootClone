using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOs.Model
{
    public class TeamResultInGame
    {
        [Key]
        public int TeamResultInGameId { get; set; } // Thêm khóa chính

        public int QuestionInGameId { get; set; }
        public int PlayerId { get; set; }
        public int SessionId { get; set; }
        public int TeamId { get; set; }
        public int Score { get; set; } // Giả sử bảng này lưu điểm số của đội cho mỗi câu hỏi

        // Quan hệ
        [ForeignKey("QuestionInGameId")]
        public QuestionInGame QuestionInGame { get; set; }

        [ForeignKey("PlayerId")]
        public Player Player { get; set; }

        [ForeignKey("SessionId")]
        public GameSession Session { get; set; }

        [ForeignKey("TeamId")]
        public Team Team { get; set; }
    }
}