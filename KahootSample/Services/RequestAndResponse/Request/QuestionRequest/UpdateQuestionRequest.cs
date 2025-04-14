namespace Services.RequestAndResponse.Request.QuestionRequest
{
    public class UpdateQuestionRequest
    {
        public int QuestionId { get; set; }
        public int QuizId { get; set; }
        public string Text { get; set; }
        public int TimeLimit { get; set; }
        public int Order { get; set; }
    }
}
