using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RequestAndResponse.Response.ResponseResponses
{
    public class ResponseResponse
    {
        public int ResponseId { get; set; }
        public int PlayerId { get; set; }
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public int ResponseTime { get; set; }
        public int Points { get; set; }
        public int Streak { get; set; }
    }
}
