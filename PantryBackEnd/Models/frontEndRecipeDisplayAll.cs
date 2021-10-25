using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PantryBackEnd.Models
{
    public class frontEndRecipeDisplayAll
    {
        public int RecipeId { get; set; }
        public string RecipeName { get; set; }

        public List<ingredients> ingredientsList { get; set; }

        public string PhotoUrl { get; set; }
        public string url { get; set; }

    }
}