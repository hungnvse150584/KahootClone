using System.ComponentModel.DataAnnotations;

namespace Services.RequestAndResponse.Request.QuestionRequest
{
    public class CreateQuestionRequest
    {
        public int QuizId { get; set; }
        public string Text { get; set; }
        public int TimeLimit { get; set; }
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Option1 is required.")]
        public string Option1 { get; set; }
        public string? Option2 { get; set; }
        public string? Option3 { get; set; }
        public string? Option4 { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "CorrectOption must be at least 1.")]
        public int CorrectOption { get; set; }
        public int OrderIndex { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Status { get; set; }
    }
}
