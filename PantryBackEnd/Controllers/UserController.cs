namespace PantryBackEnd.Controllers
{

    using Microsoft.AspNetCore.Mvc;

    public class UserController : ControllerBase
    {

        [Route("api/Users/name")]
        [HttpGet]
        public  int Get()
        {
            
            return 0;
        }
    }
}