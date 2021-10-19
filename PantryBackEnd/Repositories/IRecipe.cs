using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PantryBackEnd.Models;
namespace PantryBackEnd.Repositories
{
    public interface IRecipe
    {
        
        String createRecipe(Guid id, string recipeName, string recipeDesc);
        List<Recipe> getRecipes(Guid id);
    }
}