using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PantryBackEnd.Models;
namespace PantryBackEnd.Repositories
{
    public interface IRecipe
    {
        
        string createRecipe(Guid id, string recipeName, string recipeDesc, List<string> steps);
        
        List<frontEndRecipeDisplayAll> getRecipes(Guid id);
        frontEndRecipeStep getRecipeSteps(int recipeID, Guid id);
        List<Recipe> calculateRecipeScores(Guid accountId);
    }
}