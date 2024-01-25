using Microsoft.AspNetCore.Mvc;


namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("/")]
    public class StatusController : Controller
    {
        public StatusController() { }

        [HttpGet]
        [Route("/")]
        public IActionResult GetStatus()
        {
            return Ok(new { message = "online" });
        } 
    
    }
}
