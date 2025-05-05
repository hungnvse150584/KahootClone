using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Services.RequestAndResponse.Request.QuestionRequest
{
    public class CreateQuestionRequest : IValidatableObject
    {

        public int? QuizId { get; set; }
        public string? Text { get; set; }
        public int? TimeLimit { get; set; }

        public IFormFile? ImageFile { get; set; }
        public byte[]? ImageData { get; set; }
        public string? Option1 { get; set; }
        public string? Option2 { get; set; }
        public string? Option3 { get; set; }
        public string? Option4 { get; set; }
        public int? CorrectOption { get; set; }
        public int? OrderIndex { get; set; }
        public string? Status { get; set; }
        // Bỏ CreatedTime vì sẽ được tạo tự động phía server

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ImageFile != null && ImageData != null)
            {
                yield return new ValidationResult(
                    "Cannot provide both ImageFile and ImageData. Please provide only one.",
                    new[] { nameof(ImageFile), nameof(ImageData) });
            }

            if (CorrectOption < 1 || CorrectOption > 4)
            {
                yield return new ValidationResult(
                    "CorrectOption must be between 1 and 4.",
                    new[] { nameof(CorrectOption) });
            }
        }
    }
}