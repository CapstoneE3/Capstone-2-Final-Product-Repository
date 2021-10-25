using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PantryBackEnd.Repositories;
using PantryBackEnd.Models;
using PantryBackEnd.JwtGenerator;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

        [Route("api/browseRecipes")]
        [HttpGet]
        public ActionResult<List<Recipe>> browseRecipes(int index)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                Account user = userRepo.GetByID(userId);

                if (index < 0)
                {
                    return Ok(new List<frontEndRecipeDisplayAll>());
                }
                var recipes = recipeRepo.browseApiRecipes(index, userId);
                return Ok(recipes);

            }
            catch (Exception)
            {
                return Unauthorized();
            }
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

                if (index < 0)
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
        public async Task<ActionResult> addCustomRecipe([FromBody] CustomRecipe obj)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                obj.recipeName = "Custom " + obj.recipeName;
                var str = await recipeRepo.createRecipe(userId, obj);
                if (str.Equals("Exists") || str.Equals("Error"))
                {
                    return BadRequest(new { message = str });
                }

                return Ok(new { message = str });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Failed" });
            }
        }

        [Route("api/ApiRecipeToCustom")]
        [HttpPost]

        public async Task<ActionResult> ApiRecipeToCustom(int recipeID)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);

                CustomRecipe obj = await recipeRepo.convertToCustom(recipeID);
                if (obj == null)
                {
                    return BadRequest(new { message = "Recipe does not exist" });
                }
                var str = await recipeRepo.createRecipe(userId, obj);
                if (str.Equals("Exists") || str.Equals("Error"))
                {
                    return BadRequest(new { message = str });
                }
                return Ok(new { message = str });
            }
            catch (Exception)
            {
                return Unauthorized(new { message = "Failed" });
            }
        }
        [Route("api/RemoveFave")]
        [HttpGet]
        public async Task<ActionResult> RemoveFavouriteRecipe(int RecipeId)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                await recipeRepo.RemoveRecipe(RecipeId);

                return Ok(new { message = "Success" });
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }

        [Route("api/UpdateRecipe")]
        [HttpPost]
        public async Task<ActionResult> updateRecipe([FromBody] CustomRecipe obj, int id)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                Account user = userRepo.GetByID(userId);
                await recipeRepo.RemoveRecipe(id);
                var str = await recipeRepo.createRecipe(userId, obj);
                if (str.Equals("Exists") || str.Equals("Error"))
                {
                    return BadRequest(new { message = str });
                }

                return Ok(new { message = str });
            }
            catch (Exception)
            {
                return Ok(new { message = "Failed" });
            }
        }

        [Route("api/OnClickRecipe")]
        [HttpGet]
        public ActionResult<frontEndRecipeClickDetails> OnClickRecipe(int recipeID)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);

                //var recStep = recipeRepo.getRecipeSteps(recipeID);
                var info = recipeRepo.getInfo(recipeID);
                var clickInfo = recipeRepo.addDescToInfo(info);

                return Ok(clickInfo);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Failed" });
            }
        }


        [Route("api/getFullDetailsRecipe")]
        [HttpGet]
        public ActionResult<fullRecipeDetails> getFullDetails(int recipeId)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                Account user = userRepo.GetByID(userId);

                var clickInfo = recipeRepo.fullInfo(recipeId);

                return Ok(clickInfo);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Failed" });
            }
        }

        [Route("api/getAllProductsForIng")]
        [HttpGet]
        public ActionResult<Ingredient> getAllProductsForIng(int ingId)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                return Ok(recipeRepo.getAllProductsForIng(ingId));
            }
            catch (Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException)
            {
                return Unauthorized();
            }
        }
        [Route("api/AllIngedients")]
        [HttpGet]
        public ActionResult<List<AllIngredients>> getAllIngrdient()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                return Ok(recipeRepo.AllIngridient());
            }
            catch (Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException)
            {
                return Unauthorized();
            }
        }
        [Route("api/testRecipeScoreLogic")]
        [HttpGet]
        public ActionResult<List<frontEndRecipeDisplayAll>> calculateRecipeScores()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer); ;

                Account user = userRepo.GetByID(userId);

                var goodRecipes = recipeRepo.calculateRecipeScores(userId);
                var returnObj = recipeRepo.displayRecommendation(goodRecipes);
                return Ok(returnObj);

            }
            catch (Exception)
            {
                return Ok(new { message = "Failed" });
            }
        }

        // [Route("api/addProductTest")]
        // [HttpPost]
        // public ActionResult addProductTest(string itemId, string quantity, string category, string name, string searchtag, int ingredientId)
        // {
        //     string str = recipeRepo.addProductTest(itemId, quantity, category, name, searchtag, ingredientId);
        //     return Ok(str);
        // }

        // [Route("api/getProductDTTest")]
        // [HttpGet]
        // public ActionResult<List<ProductDt>> getProductDTTest()
        // {
        //     try
        //     {
        //         var jwt = Request.Cookies["jwt"];
        //         var token = service.Verification(jwt);
        //         Guid userId = Guid.Parse(token.Issuer);
        //         Account user = userRepo.GetByID(userId);

        //         var obj = recipeRepo.getProductDTTest(userId);

        //         return Ok(obj);

        //     }
        //     catch (Exception)
        //     {
        //         return Ok(new { message = "Failed" });
        //     }
        // }
    }
}