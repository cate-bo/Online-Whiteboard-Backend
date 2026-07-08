using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private Dictionary<int, WhiteboardState> _openWhiteboards;

        public WhiteboardStatemachine(IHttpContextAccessor httpContext, IServiceProvider sp)
        {
            _httpContext = httpContext;
            _sp = sp;
            _openWhiteboards = new Dictionary<int, WhiteboardState>();
        }
        private int _bla = 0;
        public int GiveBla()
        {
            _bla++;
            return _bla;
        }

        public async Task<IdAndNameWrapper> CreateWhiteboard(string name, bool isPublic, AspNetUsers user)
        {
            IdAndNameWrapper newBoard = new IdAndNameWrapper();
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
                    newBoard.Name = board.WhiName;
                    newBoard.Id = board.WhiIdPk;
                }
                catch (Exception ex)
                {
                    newBoard.Id = 0;
                }
            }
            return newBoard;
        }

        public async Task<OpenWhiteboardResponse> ConnectToWhiteboard(Whiteboard board, AspNetUsers? user, string connectionId)
        {
            await CloseWhiteboard(connectionId);
            //_openWhiteboards.TryAdd(board, new WhiteboardConnectionsWrapper());
            board = GetWhiteboard(board.WhiIdPk);
            if (_openWhiteboards[board.WhiIdPk] == null)
            {
                _openWhiteboards[board.WhiIdPk] = new WhiteboardState(board);
            }

            return _openWhiteboards[board.WhiIdPk].Connect(user, connectionId);
        }

        private async Task CloseWhiteboard(string connectionId)
        {
            foreach (var thing in _openWhiteboards)
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
                return context.Whiteboard.Where(w => w.WhiIdPk == id)
                    .Include(w => w.WhiBesitzerIdFkNavigation)
                    .Include(w => w.BeaNutzerIdFk)
                    .Include(w => w.Bild)
                    .Include(w => w.Text)
                    .First();
            }
        }
    }
}
