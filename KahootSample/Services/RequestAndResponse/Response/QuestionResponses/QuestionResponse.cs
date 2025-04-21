namespace Services.RequestAndResponse.Response.QuestionResponses
{
    public class QuestionResponse
    {
        public int QuestionId { get; set; }
        public int QuizId { get; set; }
        public string Text { get; set; }
        public int TimeLimit { get; set; }
        public string ImageUrl { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public int CorrectOption { get; set; }
        public int OrderIndex { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Status { get; set; }
    }
}
