using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PantryBackEnd.Repositories;
using PantryBackEnd.Models;
using PantryBackEnd.JwtGenerator;
using Microsoft.AspNetCore.Mvc;

namespace PantryBackEnd.Controllers
{
    public class RecipeController : ControllerBase
    {
        private IRecipe recipeRepo;
        private JwtService service;
        private IUserRepo userRepo;

        public RecipeController(IRecipe context, JwtService service, IUserRepo userRepo)
        {
            this.recipeRepo = context;
            this.service = service;
            this.userRepo = userRepo;

        }

        
        [Route("api/getUserRecipes")]
        [HttpGet]
        public ActionResult<List<Recipe>> getAPIRecipe()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                Account user = userRepo.GetByID(userId);


                var recipes = recipeRepo.getRecipes(userId);
                return Ok(recipes);

            }
            catch(Exception)
            {
                return Unauthorized();
            }
            
        }
    }
}