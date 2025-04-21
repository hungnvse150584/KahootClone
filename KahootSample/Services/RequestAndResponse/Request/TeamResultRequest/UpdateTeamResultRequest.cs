using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RequestAndResponse.Request.TeamResultRequest
{
    public class UpdateTeamResultRequest
    {
        public int TeamResultInGameId { get; set; }
        public int QuestionInGameId { get; set; }
        public int SessionId { get; set; }
        public int TeamId { get; set; }
        public int Score { get; set; }
    }
}
