using Microsoft.AspNetCore.Identity;
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

        public WhiteboardStatemachine(IHttpContextAccessor httpContext, IServiceProvider sp)
        {
            _httpContext = httpContext;
            _sp = sp;
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
    }
}
