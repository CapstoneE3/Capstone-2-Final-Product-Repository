using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PantryBackEnd.Models
{
    public class fullRecipeDetails
    {

        public int RecipeId { get; set; }
        public string RecipeName { get; set; }

        public List<customIngredients> ingredientsList  { get; set; }

        public string PhotoUrl { get; set; }
        public string desc { get; set;} 
        public frontEndRecipeStep steps { get; set;}

    }
}