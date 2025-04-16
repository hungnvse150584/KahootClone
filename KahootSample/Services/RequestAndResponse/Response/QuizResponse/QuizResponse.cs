using System;
using System.Collections.Generic;
using Services.RequestAndResponse.Response.QuestionResponses;

namespace Services.RequestAndResponse.Response.QuizResponses
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
        public List<QuestionResponse> Questions { get; set; }
        
    }
}
