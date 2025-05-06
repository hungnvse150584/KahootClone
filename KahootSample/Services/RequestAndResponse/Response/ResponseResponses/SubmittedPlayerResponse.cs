using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RequestAndResponse.Response.ResponseResponses
{
    public class SubmittedPlayerResponse
    {
        public int PlayerId { get; set; }
        public string Nickname { get; set; }
        public int QuestionInGameId { get; set; }
        public string SelectedOptions { get; set; }
        public int Score { get; set; }
        public bool IsCorrect { get; set; }
    }
}
