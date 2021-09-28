using System;
using System.Collections.Generic;

#nullable disable

namespace PantryBackEnd.Models
{
    public partial class Recipe
    {
        public Recipe()
        {
            RecipeIngredients = new HashSet<RecipeIngredient>();
            RecipeLists = new HashSet<RecipeList>();
        }

        public int RecipeId { get; set; }
        public string RecipeDescription { get; set; }
        public string RecipeName { get; set; }

        public virtual RecipeStep RecipeStep { get; set; }
        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; }
        public virtual ICollection<RecipeList> RecipeLists { get; set; }
    }
}
