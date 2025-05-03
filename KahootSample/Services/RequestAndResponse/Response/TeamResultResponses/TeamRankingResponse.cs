
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RequestAndResponse.Response.TeamResultResponses
{
    public class TeamRankingResponse
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int TotalScore { get; set; }
        public int Rank { get; set; }
    }

}
