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
        [Route("api/addRecipe")]
        [HttpPost]
        public void addRecipe()
        {

        }
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
        public ActionResult<List<Recipe>> getAPIRecipe(int index)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);

                if(index < 0)
                {
                    return Ok(new List<frontEndRecipeDisplayAll>());
                }
                var recipes = recipeRepo.getRecipes(userId, index);
                return Ok(recipes);

            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }

        [Route("api/addCustomRecipe")]
        [HttpPost]
        public ActionResult addCustomRecipe(string recipeName, string recipeDesc, List<String> steps, List<RecipeStep> ingredients)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);

                var str = recipeRepo.createRecipe(userId, recipeName, recipeDesc, steps);

                return Ok(new { message = str });
            }
            catch (Exception)
            {
                return Ok(new { message = "Failed" });
            }
        }

        [Route("api/getRecipeSteps")]
        [HttpGet]
        public ActionResult<List<RecipeStep>> getRecipeSteps(int recipeID)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);

                var recStep = recipeRepo.getRecipeSteps(recipeID, userId);

                return Ok(recStep);
            }
            catch (Exception)
            {
                return Ok(new { message = "Failed" });
            }
        }

        [Route("api/testRecipeScoreLogic")]
        [HttpGet]
        public ActionResult<List<Recipe>> calculateRecipeScores()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                Account user = userRepo.GetByID(userId);

                var goodRecipes = recipeRepo.calculateRecipeScores(userId);

                return Ok(goodRecipes); 

            }
            catch (Exception)
            {
                 return Ok(new { message = "Failed" });
            }
        }
    }
}