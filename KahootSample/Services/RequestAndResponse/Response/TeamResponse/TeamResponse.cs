using Services.RequestAndResponse.Response;
using Services.RequestAndResponse.Response.TeamMemberResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RequestAndResponse.Response.TeamResponse
{
    public class TeamResponse
    {
        public int TeamId { get; set; }
        public int SessionId { get; set; }
        public string Name { get; set; }
        public double TotalScore { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<TeamMemberResponse> TeamMembers { get; set; }
    }
}
