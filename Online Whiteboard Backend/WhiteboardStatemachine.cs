using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Online_Whiteboard_Backend.Models;
using System.Drawing;
using System.Threading.Tasks;
using System.Timers;

namespace Online_Whiteboard_Backend
{
    public class WhiteboardStatemachine
    {

        private readonly IHttpContextAccessor _httpContext;
        private readonly IServiceProvider _sp;
        private Dictionary<Whiteboard, WhiteboardConnectionsWrapper> _openWhiteboards;

        public WhiteboardStatemachine(IHttpContextAccessor httpContext, IServiceProvider sp)
        {
            _httpContext = httpContext;
            _sp = sp;
            _openWhiteboards = new Dictionary<Whiteboard, WhiteboardConnectionsWrapper>();
        }
        private int _bla = 0;
        public int GiveBla()
        {
            _bla++;
            return _bla;
        }

        public async Task<int> CreateWhiteboard(string name, bool isPublic, AspNetUsers user)
        {
            int boardID = 0;
            using (var scope = _sp.CreateScope())
            {
                var whiteboardContext = scope.ServiceProvider.GetRequiredService<WhiteboardContext>();
                var drawing = new Bitmap(5000, 5000);
                Whiteboard board = new Whiteboard();
                board.WhiName = name;
                board.WhiIstÖffentlich = isPublic;
                board.WhiBesitzerIdFk = user.Nutzer.NutIdPk;
                var stream = new MemoryStream();
                drawing.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                board.WhiZeichnung = stream.ToArray();
                whiteboardContext.Whiteboard.Add(board);
                try
                {
                    await whiteboardContext.SaveChangesAsync();
                    Console.WriteLine(board.WhiIdPk);
                    boardID = board.WhiIdPk;
                }
                catch (Exception ex)
                {

                }
            }
            return boardID;
        }

        public async Task<bool> OpenWhiteboard(int whiteboardId, AspNetUsers user, string connectionId)
        {
            Whiteboard board = GetWhiteboard(whiteboardId);
            await CloseWhiteboard(connectionId);
            _openWhiteboards.TryAdd(board, new WhiteboardConnectionsWrapper());
            if (user.Nutzer == board.WhiBesitzerIdFkNavigation)
            {
                _openWhiteboards[board].ConnectOwner(connectionId);
                return true;
            }
            else if (board.BeaNutzerIdFk.Contains(user.Nutzer))
            {
                _openWhiteboards[board].ConnectEditor(connectionId);
                return true;
            }
            else
            {
                return await OpenWhiteboard(whiteboardId, connectionId);
            }
        }

        public async Task<bool> OpenWhiteboard(int whiteboardId, string connectionId)
        {
            Whiteboard board = GetWhiteboard(whiteboardId);
            await CloseWhiteboard(connectionId);
            _openWhiteboards.TryAdd(board, new WhiteboardConnectionsWrapper());
            if (board.WhiIstÖffentlich)
            {
                _openWhiteboards[board].ConnectViewer(connectionId);
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task CloseWhiteboard(string connectionId)
        {
            foreach(var thing in _openWhiteboards)
            {
                thing.Value.Dissconnect(connectionId);
                if (!thing.Value.HasConnections)
                {
                    _openWhiteboards.Remove(thing.Key);
                    using (var scope = _sp.CreateScope())
                    {
                        WhiteboardContext context = scope.ServiceProvider.GetRequiredService<WhiteboardContext>();
                        context.Update(thing.Key);
                        await context.SaveChangesAsync();
                    }
                }
            }
            

        }



        private Whiteboard GetWhiteboard(int id)
        {
            using (var scope = _sp.CreateScope())
            {
                WhiteboardContext context = scope.ServiceProvider.GetRequiredService<WhiteboardContext>();
                return context.Whiteboard.Where(w => w.WhiIdPk == id).Include(w => w.WhiBesitzerIdFkNavigation).Include(w => w.BeaNutzerIdFk).First();
            }
        }
    }
}
