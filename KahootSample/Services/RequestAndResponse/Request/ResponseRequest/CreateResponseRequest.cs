using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RequestAndResponse.Request.ResponseRequest
{
    public class CreateResponseRequest
    {
        [Required(ErrorMessage = "PlayerId is required")]
        public int PlayerId { get; set; }

        [Required(ErrorMessage = "QuestionInGameId is required")]
        public int QuestionInGameId { get; set; } // Sửa từ QuestionId

        [Required(ErrorMessage = "SelectedOption is required")]
        [Range(1, 4, ErrorMessage = "SelectedOption must be between 1 and 4")]
        public int SelectedOption { get; set; } // Sửa từ AnswerId

        [Range(0, int.MaxValue, ErrorMessage = "ResponseTime must be a non-negative number")]
        public int ResponseTime { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Score must be a non-negative number")]
        public int Score { get; set; } // Sửa từ Points

        [Range(0, int.MaxValue, ErrorMessage = "Streak must be a non-negative number")]
        public int Streak { get; set; }

        public int Rank { get; set; }
    }
}
