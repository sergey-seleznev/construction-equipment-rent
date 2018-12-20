using Microsoft.AspNetCore.Mvc;

namespace ConstructionEquipmentRent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok();
    }
}
