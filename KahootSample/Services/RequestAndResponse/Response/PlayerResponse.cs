using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RequestAndResponse.Response
{
    public class PlayerResponse
    {
        public int PlayerId { get; set; }
        public string Nickname { get; set; }
        public int SessionId { get; set; }
        public int? UserId { get; set; }
    }
}
