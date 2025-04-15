using System;
using System.ComponentModel.DataAnnotations;

namespace Services.RequestAndResponse.PlayerRequest
{
    public class CreatePlayerRequest
    {
        public int? SessionId { get; set; }

        public int? UserId { get; set; }

        [Required, MaxLength(50)]
        public string Nickname { get; set; }
    }
}
