using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace PantryBackEnd.Controllers
{
    public class RecipeController : ControllerBase
    {
        [Route("api/addRecipe")]
        [HttpPost]
        public void addRecipe()
        {

        }
    }
}