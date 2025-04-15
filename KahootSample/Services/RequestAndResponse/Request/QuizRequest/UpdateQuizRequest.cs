using System;
using System.ComponentModel.DataAnnotations;

namespace Services.RequestAndResponse.Request
{
    public class UpdateQuizRequest
    {
        [Key]
        public int QuizId { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(10)]
        public string Pin { get; set; }

        [MaxLength(200)]
        public string QrCode { get; set; }
    }
}
