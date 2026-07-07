using Online_Whiteboard_Backend.Models;

namespace Online_Whiteboard_Backend
{
    public class OpenWhiteboardResponse
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public List<UserWrapper> CurrentUsers { get; set; }


    }
}
