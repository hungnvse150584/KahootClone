using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RequestAndResponse.Response.SummaryResponse
{
    public class QuestionSummary
    {
        public int QuestionInGameId { get; set; }
        public int OrderIndex { get; set; }
        public string Text { get; set; }
        public int TotalMembers { get; set; }
        public string CorrectOptions { get; set; }
        public string Status { get; set; } // From Question
        public double CorrectAnswerRate { get; set; }
    }
}
