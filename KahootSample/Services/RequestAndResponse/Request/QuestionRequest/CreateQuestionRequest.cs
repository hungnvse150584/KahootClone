namespace Services.RequestAndResponse.Request.QuestionRequest
{
    public class CreateQuestionRequest
    {
        public int QuizId { get; set; }
        public string Text { get; set; }
        public int TimeLimit { get; set; }
        public int Order { get; set; }
    }
}
