using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetTestMessage()
        {
            return Ok(new { message = "Backend ve Frontend başarıyla bağlandı!" });
        }
    }
}