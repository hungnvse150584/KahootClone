namespace Services.RequestAndResponse.Response.QuestionResponses
{
    public class QuestionResponse
    {
        public int QuestionId { get; set; }
        public int QuizId { get; set; }
        public string Text { get; set; }
        public int TimeLimit { get; set; }
        public int Order { get; set; }

    }
}
