using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RequestAndResponse.Request.PlayerRequest
{
    public class UpdatePlayerRequest
    {
        [Required]
        public int PlayerId { get; set; }

        [Required]
        public int SessionId { get; set; }

        [MaxLength(50)]
        public string Nickname { get; set; }

        public int? Score { get; set; }

        public int? Ranking { get; set; }

        public int? UserId { get; set; }

        public int? TeamId { get; set; }
    }
}
