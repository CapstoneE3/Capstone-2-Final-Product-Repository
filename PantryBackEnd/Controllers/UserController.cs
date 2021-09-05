using System.Threading.Tasks;
namespace PantryBackEnd.Controllers
{

    using Microsoft.AspNetCore.Mvc;

    public class UserController : ControllerBase
    {

        [Route("api/Users/name")]
        [HttpGet]
        public async Task<ActionResult<bool>>login()
        {
            return true;
        }
    }
}