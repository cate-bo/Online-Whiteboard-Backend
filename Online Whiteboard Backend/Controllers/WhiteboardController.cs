using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Online_Whiteboard_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhiteboardController : ControllerBase
    {
        private readonly WhiteboardStatemachine _statemachine;
        private readonly UserManager<IdentityUser> _userManager;

        public WhiteboardController(WhiteboardStatemachine statemachine, UserManager<IdentityUser> userManager)
        {
            _statemachine = statemachine;
            _userManager = userManager;
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
            await _statemachine.CreateWhiteboard(request.Name, request.IsPublic);
            return 0;
        }
    }
}
