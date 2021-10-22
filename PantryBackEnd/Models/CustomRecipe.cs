using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PantryBackEnd.Models
{
    public class CustomRecipe
    {

        public string recipeName;
        public string recipeDesc;
        public List<string> steps;

        public string photo;
        public string url;
       
        public List<customIngredients> ingredients;

    }
}