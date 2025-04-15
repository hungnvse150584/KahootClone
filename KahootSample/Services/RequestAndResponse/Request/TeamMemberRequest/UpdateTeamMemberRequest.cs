using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RequestAndResponse.Request.TeamMemberRequest
{
    public class UpdateTeamMemberRequest
    {
        [Required(ErrorMessage = "TeamId is required")]
        public int TeamId { get; set; }

        [Required(ErrorMessage = "PlayerId is required")]
        public int PlayerId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Score must be a non-negative number")]
        public int Score { get; set; }
    }
}
