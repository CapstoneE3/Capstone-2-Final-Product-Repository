using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PantryBackEnd.Repositories
{
    public interface IRecipe
    {
        
        void randomRecipe();
        void recipeFormat(dynamic details);
    }
}