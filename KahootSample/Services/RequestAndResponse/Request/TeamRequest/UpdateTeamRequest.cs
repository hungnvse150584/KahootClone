using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RequestAndResponse.Request.TeamRequest
{
    public class UpdateTeamRequest
    {
        //[Key]
        [Key]
        public int TeamId { get; set; }

        [Required]
        public int SessionId { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }
    }
}
