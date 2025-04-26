using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RequestAndResponse.Request.PlayerRequest
{
    public class CreatePlayerSignalRRequest
    {
        [Required]
        public int PlayerId { get; set; } // Thêm PlayerId

        [Required]
        public int? SessionId { get; set; }

        [Required, MaxLength(50)]
        public string Nickname { get; set; }
    }
}
