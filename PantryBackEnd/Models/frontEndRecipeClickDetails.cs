using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PantryBackEnd.Models
{
    public class frontEndRecipeClickDetails
    {
        public int RecipeId { get; set; }
        public string RecipeName { get; set; }
        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; }
        public string RecipeDescription { get; set; }
        public string PhotoUrl { get; set; }
    }
}