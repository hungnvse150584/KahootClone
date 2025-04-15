using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RequestAndResponse.Request.ScoreRequest
{
    public class CreateScoreRequest
    {
        public int PlayerId { get; set; }
        public int SessionId { get; set; }
        public int TotalPoints { get; set; }
    }
}
