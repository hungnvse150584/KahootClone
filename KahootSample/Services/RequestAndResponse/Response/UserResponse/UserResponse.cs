namespace Services.RequestAndResponse.Response.UserResponse
{
    public class UserResponse
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Role { get; set; } // Added Role
    }
}