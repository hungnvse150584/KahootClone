using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RequestAndResponse.Response.SummaryResponse
{
    public class TeamSummary
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public double TotalScore { get; set; } // Matches Team.TotalScore
    }
}
