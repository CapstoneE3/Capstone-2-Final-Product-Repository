using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PantryBackEnd.Models
{
    public class CustomRecipe
    {

        public string recipeName;
        public string desc;
        public List<string> steps;

        public string photoUrl;
        public string url;

        public List<customIngredients> ingredientsList;

    }
}