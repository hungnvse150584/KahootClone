using System;
using System.Collections.Generic;

namespace Services.RequestAndResponse.Response
{
    public class QuizResponse
    {
        public int QuizId { get; set; }
        public string Title { get; set; }
        public int CreatedBy { get; set; }
        public string Pin { get; set; }
        public string QrCode { get; set; }
        public DateTime CreatedAt { get; set; }

        // Optional: Include related data
        public List<QuestionResponse> Questions { get; set; } = new();
        
    }
}
