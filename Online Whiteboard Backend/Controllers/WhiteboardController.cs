using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Whiteboard_Backend.Models;
using System.Drawing;

namespace Online_Whiteboard_Backend.Controllers
{
    [Route("api/[controller]")]
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
        public ActionResult<int> Get()
        {
            return _statemachine.GiveBla();
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
