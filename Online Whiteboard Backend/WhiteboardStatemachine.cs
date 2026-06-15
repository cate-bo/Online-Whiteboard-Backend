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

        public async Task CreateWhiteboard(string name, bool isPublic)
        {
            using (var scope = _sp.CreateScope())
            {
                //---------------------------------------------prolly need to pass services from caller ---------------------------------------------------------------

                var whiteboardContext = scope.ServiceProvider.GetRequiredService<WhiteboardContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var identityUser = await userManager.GetUserAsync(_httpContext.HttpContext.User);
                var user = whiteboardContext.Nutzer.First(u => u.NutAspUserIdFk == identityUser.Id);
                var drawing = new Bitmap(5000, 5000);
                Whiteboard board = new Whiteboard();
                board.WhiName = name;
                board.WhiIstÖffentlich = isPublic;
                board.WhiBesitzerIdFk = user.NutIdPk;
                var stream = new MemoryStream();
                drawing.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                board.WhiZeichnung = stream.ToArray();
                whiteboardContext.Whiteboard.Add(board);
                try
                {
                    whiteboardContext.SaveChangesAsync();
                    Console.WriteLine(board.WhiIdPk);
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
