using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RequestAndResponse.Request.ResponseRequest
{
    public class UpdateResponseRequest
    {
        [Required(ErrorMessage = "PlayerId is required")]
        public int PlayerId { get; set; }

        [Required(ErrorMessage = "QuestionId is required")]
        public int QuestionId { get; set; }

        [Required(ErrorMessage = "AnswerId is required")]
        public int AnswerId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "ResponseTime must be a non-negative number")]
        public int ResponseTime { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Points must be a non-negative number")]
        public int Points { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Streak must be a non-negative number")]
        public int Streak { get; set; }
    }
}
