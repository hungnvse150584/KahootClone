using System;
using System.ComponentModel.DataAnnotations;
using static BOs.Model.Quiz;

namespace Services.RequestAndResponse.Request
{
    public class CreateQuizRequest
    {
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        [Required]
        [RegularExpression("Active|Inactive", ErrorMessage = "Status must be 'Active' or 'Inactive'")]
        public string Status { get; set; }

    }
}
