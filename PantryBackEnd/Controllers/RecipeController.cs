using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PantryBackEnd.Repositories;


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
        private IRecipe recipeRepo;

        public RecipeController(IRecipe context)
        {
            this.recipeRepo = context;
        }
        /*
        [Route("api/getAPIRecipe")]
        [HttpGet]
        public ActionResult getAPIRecipe()
        {
            
            recipeRepo.randomRecipe();
            return Ok(new { message = "Success" });
            
            
        }*/
    }
}