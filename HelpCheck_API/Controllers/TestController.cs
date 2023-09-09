using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HelpCheck_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public TestController()
        {

        }

        [HttpGet("[action]")]
        public async Task<IActionResult> TestApi()
        {
            return Ok("Connected.");
        }
    }
}
