using System;
using System.Collections.Generic;

#nullable disable

namespace PantryBackEnd.Models
{
    public partial class RecipeIngredient
    {
        public int RecipeId { get; set; }
        public int IngredientId { get; set; }
        public float Amount { get; set; }
        public string UnitOfMeasure { get; set; }
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public string LinkProd { get; set; }

        public virtual Product LinkProdNavigation { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}
