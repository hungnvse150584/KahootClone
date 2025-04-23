namespace Services.RequestAndResponse.Response.PlayerResponse
{
    public class PlayerResponse
    {
        public int PlayerId { get; set; }
        public int SessionId { get; set; }
        public int? UserId { get; set; }
        public string Nickname { get; set; }
        public DateTime JoinedAt { get; set; }
        public int? Score { get; set; }

    }
}
