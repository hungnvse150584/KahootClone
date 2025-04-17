using Services.RequestAndResponse.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RequestAndResponse.Response.TeamResultInGameResponse
{
    public class TeamResultInGameResponse
    {
        public int TeamResultInGameId { get; set; }
        public int QuestionInGameId { get; set; }
        public int PlayerId { get; set; }
        public int SessionId { get; set; }
        public int TeamId { get; set; }
        public int Score { get; set; }

    }
}
