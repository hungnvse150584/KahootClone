using System;
using System.ComponentModel.DataAnnotations;

namespace Services.RequestAndResponse.Request
{
    public class CreateQuizRequest
    {
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public int CreatedBy { get; set; }

    }
}
