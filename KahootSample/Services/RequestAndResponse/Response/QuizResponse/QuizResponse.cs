using System;
using System.Collections.Generic;
using Services.RequestAndResponse.Response.QuestionResponses;
using static BOs.Model.Quiz;

namespace Services.RequestAndResponse.Response.QuizResponses
{
    public class QuizResponse
    {
        public int QuizId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }

        // Optional: Include related data
        public List<QuestionResponse> Questions { get; set; }
        
    }
}
