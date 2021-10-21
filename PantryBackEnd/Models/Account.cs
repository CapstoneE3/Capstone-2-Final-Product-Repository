using System;
using System.Collections.Generic;

#nullable disable

namespace PantryBackEnd.Models
{
    public partial class Account
    {
        public Account()
        {
            InventoryLists = new HashSet<InventoryList>();
            RecipeLists = new HashSet<RecipeList>();
            ShoppingLists = new HashSet<ShoppingList>();
            Subscriptions = new HashSet<Subscription>();
        }

        public Guid AccId { get; set; }
        public string Firstname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Lastname { get; set; }

        public virtual Admin Admin { get; set; }
        public virtual ICollection<InventoryList> InventoryLists { get; set; }
        public virtual ICollection<RecipeList> RecipeLists { get; set; }
        public virtual ICollection<ShoppingList> ShoppingLists { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
    }
}
