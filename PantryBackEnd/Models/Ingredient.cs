using System;
using System.Collections.Generic;

#nullable disable

namespace PantryBackEnd.Models
{
    public partial class Ingredient
    {
        public Ingredient()
        {
            Products = new HashSet<Product>();
        }

        public int IngredientId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
