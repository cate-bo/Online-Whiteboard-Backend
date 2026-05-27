using Microsoft.AspNetCore.SignalR;

namespace Online_Whiteboard_Backend.Hubs
{
    public class WhiteboardHub : Hub
    {
        private readonly WhiteboardStatemachine _statemachine;

        public WhiteboardHub(WhiteboardStatemachine statemachine)
        {
            _statemachine = statemachine;
        }
    }
}
