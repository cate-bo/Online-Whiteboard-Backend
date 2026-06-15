namespace Online_Whiteboard_Backend
{
    public class CreateWhiteboardRequest
    {
        public required string Name { get; set; }
        public required bool IsPublic { get; set; }
    }
}
