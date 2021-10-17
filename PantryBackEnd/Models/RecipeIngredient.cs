using System;
using System.Collections.Generic;

#nullable disable

namespace PantryBackEnd.Models
{
    public partial class RecipeIngredient
    {
        public int RecipeId { get; set; }
        public string IngredientId { get; set; }
        public int Amount { get; set; }
        public string UnitOfMeasure { get; set; }
        public string Name { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
