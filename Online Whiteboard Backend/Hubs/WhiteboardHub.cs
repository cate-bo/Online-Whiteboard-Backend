using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Online_Whiteboard_Backend.Models;

namespace Online_Whiteboard_Backend.Hubs
{
    public class WhiteboardHub : Hub
    {
        private readonly WhiteboardStatemachine _statemachine;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly WhiteboardContext _context;

        public WhiteboardHub(WhiteboardStatemachine statemachine, UserManager<IdentityUser> userManager, WhiteboardContext context)
        {
            _statemachine = statemachine;
            _userManager = userManager;
            _context = context;
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

        public async Task<bool> OpenWhiteboard(int id)
        {
            bool success = false;
            if (Context.User != null)
            {
                string user_id = _userManager.GetUserId(Context.User);
                AspNetUsers user = _context.AspNetUsers.Where(u => u.Id == user_id).Include(u => u.Nutzer).First();
                 success = await _statemachine.OpenWhiteboard(id, user, Context.ConnectionId);
            }
            else
            {
                success = await _statemachine.OpenWhiteboard(id, Context.ConnectionId);
            }
            Console.WriteLine("amogus");
            Console.WriteLine(success);
            return success;
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine("connecti");
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
