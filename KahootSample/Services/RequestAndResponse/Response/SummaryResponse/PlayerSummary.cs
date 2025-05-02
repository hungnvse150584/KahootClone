using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RequestAndResponse.Response.SummaryResponse
{
    public class PlayerSummary
    {
        public int PlayerId { get; set; }
        public string Nickname { get; set; }
        public int? TeamId { get; set; }
        public int TotalScore { get; set; }
    }
}
