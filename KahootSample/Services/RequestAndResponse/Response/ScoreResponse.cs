using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RequestAndResponse.Response
{
    public class ScoreResponse
    {
        public int ScoreId { get; set; }
        public int PlayerId { get; set; }
        public int SessionId { get; set; }
        public int TotalPoints { get; set; }
    }
}
