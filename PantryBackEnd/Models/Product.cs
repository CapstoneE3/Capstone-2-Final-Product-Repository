using System;
using System.Collections.Generic;

#nullable disable

namespace PantryBackEnd.Models
{
    public partial class Product
    {
        public Product()
        {
            InventoryLists = new HashSet<InventoryList>();
            RecipeLists = new HashSet<RecipeList>();
            ShoppingLists = new HashSet<ShoppingList>();
        }

        public string ItemId { get; set; }
        public string Quantity { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }

        public virtual ICollection<InventoryList> InventoryLists { get; set; }
        public virtual ICollection<RecipeList> RecipeLists { get; set; }
        public virtual ICollection<ShoppingList> ShoppingLists { get; set; }
    }
}
