using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PantryBackEnd.Models;
namespace PantryBackEnd.Repositories
{
    public interface IRecipe
    {
        
        string createRecipe(Guid id, CustomRecipe obj);
        CustomRecipe convertToCustom(int recipeID);
        List<frontEndRecipeDisplayAll> getRecipes(Guid id, int index);
        frontEndRecipeStep getRecipeSteps(int recipeID, Guid id);
        List<Recipe> calculateRecipeScores(Guid accountId);

        String addProductTest(string itemId, string quantity, string category, string name, string searchtag, int ingredientId);

        TillShoppingItems getProductDTTest(Guid userId);
    }
}