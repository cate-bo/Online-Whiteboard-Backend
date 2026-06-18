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

        public async Task Test(string text)
        {
            Console.WriteLine(text);
            Clients.Caller.SendAsync("test", "hello");
        }

        public async Task CreateGroup()
        {
            //var thing = await Groups.AddToGroupAsync("v", "sd");
        }
    }
}
