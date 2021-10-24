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
        public ActionResult addCustomRecipe([FromBody] CustomRecipe obj)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);

                var str = recipeRepo.createRecipe(userId, obj);

                return Ok(new { message = str });
            }
            catch (Exception)
            {
                return Ok(new { message = "Failed" });
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
                Account user = userRepo.GetByID(userId);

                CustomRecipe obj = await recipeRepo.convertToCustom(recipeID);
                if (obj == null)
                {
                    return BadRequest(new { message = "Recipe does not exist" });
                }
                var str = await recipeRepo.createRecipe(userId, obj);

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

        [Route("api/getAllProductsForIng")]
        [HttpGet]
        public ActionResult<Ingredient> getAllProductsForIng(int ingId)
        {
            return recipeRepo.getAllProductsForIng(ingId);
        }
        [Route("api/AllIngedients")]
        [HttpGet]
        public ActionResult<List<Ingredient>> getAllIngrdient()
        {
            return Ok(recipeRepo.AllIngridient());
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