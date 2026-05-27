using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Online_Whiteboard_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhiteboardController : ControllerBase
    {
        private readonly WhiteboardStatemachine _statemachine;

        public WhiteboardController(WhiteboardStatemachine statemachine)
        {
            _statemachine = statemachine;
        }

        [HttpGet]
        public ActionResult<int> Get()
        {
            return _statemachine.GiveBla();
        }
    }
}
