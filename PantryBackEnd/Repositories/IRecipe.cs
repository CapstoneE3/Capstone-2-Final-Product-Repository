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
        Ingredient getAllProductsForIng(int ingId);
        List<frontEndRecipeDisplayAll> getRecipes(Guid id, int index);
        List<frontEndRecipeDisplayAll> browseApiRecipes(int index, Guid id);
        frontEndRecipeStep getRecipeSteps(int recipeID, Guid id);
        List<Recipe> calculateRecipeScores(Guid accountId);
        List<Ingredient> AllIngridient();

        // String addProductTest(string itemId, string quantity, string category, string name, string searchtag, int ingredientId);

        // TillShoppingItems getProductDTTest(Guid userId);
    }
}