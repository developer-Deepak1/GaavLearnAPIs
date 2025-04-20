using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GaavLearnAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [Route("GetUsersList")]
        public IActionResult GetUsersList()
        {
            List<string> users= new List<string>();
            users.Add("Deepak");
            users.Add("Pragya");
            return Ok(users);
        }

    }
}
