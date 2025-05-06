using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Services.RequestAndResponse.Request.QuestionRequest
{
    public class UpdateQuestionRequest : IValidatableObject
    {

   

        public int? QuestionId { get; set; }
        public int? QuizId { get; set; }
        public string? Text { get; set; }
        public int? TimeLimit { get; set; }

        public IFormFile? ImageFile { get; set; }
        public byte[]? ImageData { get; set; }
        public string? Option1 { get; set; }
        public string? Option2 { get; set; }
        public string? Option3 { get; set; }
        public string? Option4 { get; set; }
        public string? CorrectOptions { get; set; }
        public int? OrderIndex { get; set; }
        public string? Status { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ImageFile != null && ImageData != null)
            {
                yield return new ValidationResult(
                    "Cannot provide both ImageFile and ImageData. Please provide only one.",
                    new[] { nameof(ImageFile), nameof(ImageData) });
            }

            int optionCount = 0;
            if (!string.IsNullOrEmpty(Option1)) optionCount++;
            if (!string.IsNullOrEmpty(Option2)) optionCount++;
            if (!string.IsNullOrEmpty(Option3)) optionCount++;
            if (!string.IsNullOrEmpty(Option4)) optionCount++;

            if (CorrectOptions != null) // Allow null for updates (don't update the field)
            {
                if (string.IsNullOrEmpty(CorrectOptions))
                {
                    yield return new ValidationResult(
                        "CorrectOptions cannot be an empty string. Either provide valid values (e.g., '1' or '1,3') or omit the field.",
                        new[] { nameof(CorrectOptions) });
                }
                else
                {
                    List<int> options = new();
                    bool parseError = false;

                    try
                    {
                        options = CorrectOptions.Split(',')
                            .Select(opt => int.Parse(opt.Trim()))
                            .ToList();
                    }
                    catch (FormatException)
                    {
                        parseError = true;
                    }

                    if (parseError)
                    {
                        yield return new ValidationResult(
                            "CorrectOptions must contain valid integer values separated by commas (e.g., '1' or '1,3').",
                            new[] { nameof(CorrectOptions) });
                    }
                    else
                    {
                        foreach (var opt in options)
                        {
                            if (opt < 1 || opt > optionCount)
                            {
                                yield return new ValidationResult(
                                    $"CorrectOptions contains an invalid value '{opt}'. Must be between 1 and {optionCount} based on provided options.",
                                    new[] { nameof(CorrectOptions) });
                            }

                            if (opt == 1 && string.IsNullOrEmpty(Option1))
                            {
                                yield return new ValidationResult(
                                    "Option1 must be provided when CorrectOptions includes '1'.",
                                    new[] { nameof(Option1) });
                            }
                            if (opt == 2 && string.IsNullOrEmpty(Option2))
                            {
                                yield return new ValidationResult(
                                    "Option2 must be provided when CorrectOptions includes '2'.",
                                    new[] { nameof(Option2) });
                            }
                            if (opt == 3 && string.IsNullOrEmpty(Option3))
                            {
                                yield return new ValidationResult(
                                    "Option3 must be provided when CorrectOptions includes '3'.",
                                    new[] { nameof(Option3) });
                            }
                            if (opt == 4 && string.IsNullOrEmpty(Option4))
                            {
                                yield return new ValidationResult(
                                    "Option4 must be provided when CorrectOptions includes '4'.",
                                    new[] { nameof(Option4) });
                            }
                        }

                        if (options.Count != options.Distinct().Count())
                        {
                            yield return new ValidationResult(
                                "CorrectOptions contains duplicate values.",
                                new[] { nameof(CorrectOptions) });
                        }
                    }
                }
            }

            // Validate TimeLimit (if provided)
            if (TimeLimit.HasValue && TimeLimit.Value <= 0)
            {
                yield return new ValidationResult(
                    "TimeLimit must be a positive integer.",
                    new[] { nameof(TimeLimit) });
            }

            // Validate OrderIndex (if provided)
            if (OrderIndex.HasValue && OrderIndex.Value < 0)
            {
                yield return new ValidationResult(
                    "OrderIndex cannot be negative.",
                    new[] { nameof(OrderIndex) });
            }

            // Validate Status (if provided, assuming a predefined set of values)
            if (!string.IsNullOrEmpty(Status))
            {
                var validStatuses = new[] { "Active", "Inactive", "Draft" }; // Adjust based on your application
                if (!validStatuses.Contains(Status))
                {
                    yield return new ValidationResult(
                        $"Status must be one of: {string.Join(", ", validStatuses)}.",
                        new[] { nameof(Status) });
                }
            }

            yield break;
        }
    }
}
