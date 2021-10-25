using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PantryBackEnd.Models;
namespace PantryBackEnd.Repositories
{
    public interface IRecipe
    {
        List<frontEndRecipeDisplayAll> displayRecommendation(List<Recipe> recs);
        string createRecipe(Guid id, CustomRecipe obj);
        CustomRecipe convertToCustom(int recipeID);
        Ingredient getAllProductsForIng(int ingId);
        List<frontEndRecipeDisplayAll> getRecipes(Guid id, int index);
        List<frontEndRecipeDisplayAll> browseApiRecipes(int index, Guid id);
        frontEndRecipeStep getRecipeSteps(int recipeID);

        frontEndRecipeClickDetails addDescToInfo(frontEndRecipeDisplayAll recStep);
        List<Recipe> calculateRecipeScores(Guid accountId);

        frontEndRecipeDisplayAll getInfo(int recipeID);
        String addProductTest(string itemId, string quantity, string category, string name, string searchtag, int ingredientId);
        fullRecipeDetails fullInfo(int id);
        TillShoppingItems getProductDTTest(Guid userId);
    }
}