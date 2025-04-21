using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RequestAndResponse.Request.QuestionInGameRequest
{
    public class SubmitResponseRequest
    {
        public int SessionId { get; set; }
        public int PlayerId { get; set; }
        public int AnswerId { get; set; }
        public int QuestionInGameId { get; set; }
        public int ResponseTime { get; set; }
    }
}
