using Microsoft.AspNetCore.Authorization;
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


        [Authorize]
        [AllowAnonymous]
        public async Task<OpenWhiteboardResponse> OpenWhiteboard(int id)
        {
            bool success = false;
            Whiteboard board = _context.Whiteboard.Where(w => w.WhiIdPk == id).Include(w => w.WhiBesitzerIdFkNavigation).Include(w => w.BeaNutzerIdFk).First();
            if (Context.User != null)
            {
                string user_id = _userManager.GetUserId(Context.User);
                AspNetUsers user = _context.AspNetUsers.Where(u => u.Id == user_id).Include(u => u.Nutzer).First();
                if (user.Nutzer.NutIdPk == board.WhiBesitzerIdFk)
                {
                    return await _statemachine.ConnectToWhiteboard(board, user, Context.ConnectionId);
                }
                else if (board.BeaNutzerIdFk.Contains(user.Nutzer))
                {
                    return await _statemachine.ConnectToWhiteboard(board, user, Context.ConnectionId);
                }
                else if (board.WhiIstÖffentlich)
                {
                    return await _statemachine.ConnectToWhiteboard(board, null, Context.ConnectionId);
                }
                else
                {
                    throw new HubException();
                }
            }
            else if (board.WhiIstÖffentlich)
            {
                return await _statemachine.ConnectToWhiteboard(board, null, Context.ConnectionId);
            }
            else
            {
                throw new HubException();
            }
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
