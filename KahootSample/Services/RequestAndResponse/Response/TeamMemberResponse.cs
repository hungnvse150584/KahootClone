using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RequestAndResponse.Response
{
    public class TeamMemberResponse
    {
        public int TeamMemberId { get; set; }
        public int TeamId { get; set; }
        public int PlayerId { get; set; }
        public int Score { get; set; }
        public PlayerResponse Player { get; set; }
    }
}
