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
            ShoppingLists = new HashSet<ShoppingList>();
        }

        public string ItemId { get; set; }
        public string Quantity { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public string Searchtag { get; set; }
        public int IngredientId { get; set; }
        public string PhotoUrl { get; set; }

        public virtual Category CategoryNavigation { get; set; }
        public virtual Ingredient Ingredient { get; set; }
        public virtual ICollection<InventoryList> InventoryLists { get; set; }
        public virtual ICollection<ShoppingList> ShoppingLists { get; set; }
    }
}
