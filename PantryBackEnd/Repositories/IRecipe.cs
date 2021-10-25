using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PantryBackEnd.Models;
namespace PantryBackEnd.Repositories
{
    public interface IRecipe
    {
        Task RemoveRecipe(int id);
        Task<string> createRecipe(Guid id, CustomRecipe obj);
        Task<CustomRecipe> convertToCustom(int recipeID);
        List<frontEndRecipeDisplayAll> displayRecommendation(List<Recipe> recs);
        Ingredient getAllProductsForIng(int ingId);
        List<frontEndRecipeDisplayAll> getRecipes(Guid id, int index);
        List<frontEndRecipeDisplayAll> browseApiRecipes(int index, Guid id);
        frontEndRecipeStep getRecipeSteps(int recipeID);

        frontEndRecipeClickDetails addDescToInfo(frontEndRecipeDisplayAll recStep);
        List<Recipe> calculateRecipeScores(Guid accountId);
        List<AllIngredients> AllIngridient();

        frontEndRecipeDisplayAll getInfo(int recipeID);
        String addProductTest(string itemId, string quantity, string category, string name, string searchtag, int ingredientId);
        fullRecipeDetails fullInfo(int id);
        Task<List<frontEndRecipeDisplayAll>> Search(string value);
        //TillShoppingItems getProductDTTest(Guid userId);
    }
}