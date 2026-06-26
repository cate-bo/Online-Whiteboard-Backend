using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using Online_Whiteboard_Backend.Models;
using System.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Online_Whiteboard_Backend.Controllers
{
    [Route("boards")]
    [ApiController]
    public class WhiteboardController : ControllerBase
    {
        private readonly WhiteboardStatemachine _statemachine;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly WhiteboardContext _context;

        public WhiteboardController(WhiteboardStatemachine statemachine, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor, WhiteboardContext context)
        {
            _statemachine = statemachine;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        [HttpGet]
        [Authorize]
        [AllowAnonymous]
        public async Task<ActionResult<List<IdAndNameWrapper>>> Get()
        {
            List<Whiteboard> boards = new List<Whiteboard>();
            try
            {
                string id = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
                Nutzer nutzer = _context.AspNetUsers.Where(a => a.Id == id).Include(a => a.Nutzer).First().Nutzer;
                boards = _context.Whiteboard.Include(w => w.BeaNutzerIdFk).Where(w => w.WhiIstÖffentlich == true || w.BeaNutzerIdFk.Contains(nutzer) || w.WhiBesitzerIdFkNavigation == nutzer).ToList();
            }catch (Exception ex)
            {
                boards = _context.Whiteboard.Where(w => w.WhiIstÖffentlich == true).ToList();
            }

            List<IdAndNameWrapper> boardList = new List<IdAndNameWrapper>();
            foreach (Whiteboard board in boards)
            {
                boardList.Add(new IdAndNameWrapper { Id = board.WhiIdPk, Name = board.WhiName }); 
            }
            return boardList;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<int>> Post([FromBody] CreateWhiteboardRequest request)
        {
            string id = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            AspNetUsers user = _context.AspNetUsers.Where(u => u.Id == id).Include(u => u.Nutzer).First();
            await _statemachine.CreateWhiteboard(request.Name, request.IsPublic, user);
            return 0;
        }


        [HttpGet]
        [Route("test")]
        public async Task<FileResult> Thing()
        {
            Image image = Image.FromStream(new MemoryStream(_context.Whiteboard.First().WhiZeichnung));
            return File(_context.Whiteboard.First().WhiZeichnung, "application/octet-stream", "thing.png");
        }
    }
}
