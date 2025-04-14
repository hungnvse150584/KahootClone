using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs.Model
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required, MaxLength(50)]
        //[Index(IsUnique = true)]
        public string Username { get; set; }

        [Required, MaxLength(256)]
        public string Password { get; set; }

        [Required, MaxLength(100)]
        public string Email { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Quan hệ
        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
        public ICollection<Player> Players { get; set; } = new List<Player>();
    }
}
