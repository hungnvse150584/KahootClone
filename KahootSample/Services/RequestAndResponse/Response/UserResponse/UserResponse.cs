using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RequestAndResponse.Response.UserResponse
{
    public class UserResponse
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }

        // Optional: nếu bạn muốn đưa thêm dữ liệu liên quan (quizzes, players), bạn có thể thêm các response con
        // public List<QuizResponse> Quizzes { get; set; }
        // public List<PlayerResponse> Players { get; set; }
    }
}
