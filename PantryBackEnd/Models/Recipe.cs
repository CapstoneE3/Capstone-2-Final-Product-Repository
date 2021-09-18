using System;
using System.Collections.Generic;

#nullable disable

namespace PantryBackEnd.Models
{
    public partial class Recipe
    {
        public int RecipeId { get; set; }
        public string Steps { get; set; }
        public string RecipeName { get; set; }

        public virtual RecipeList RecipeList { get; set; }
    }
}
