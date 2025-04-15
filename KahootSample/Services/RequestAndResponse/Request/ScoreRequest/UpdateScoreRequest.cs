using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RequestAndResponse.Request.ScoreRequest
{
    public class UpdateScoreRequest
    {
        public int ScoreId { get; set; }
        public int TotalPoints { get; set; }
    }
}
