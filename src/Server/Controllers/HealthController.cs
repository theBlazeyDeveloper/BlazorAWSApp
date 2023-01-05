using Microsoft.AspNetCore.Mvc;

namespace BlazorAWSApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        public HealthController() : base()
        { }

        [HttpGet]
        public IActionResult Index() => Ok("App is in good shape!");
    }
}
