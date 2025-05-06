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
        public int QuestionInGameId { get; set; }
        public string SelectedOption { get; set; }
        public int ResponseTime { get; set; }
        public int Score { get; set; }
        public int Streak { get; set; }
        public int Rank { get; set; }
    }
}
