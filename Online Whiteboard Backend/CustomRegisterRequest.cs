namespace Online_Whiteboard_Backend
{
    public class CustomRegisterRequest
    {
        public required string Email { get; init; }
        public required string Password { get; init; }
        public required string Username { get; set; }
    }
}
