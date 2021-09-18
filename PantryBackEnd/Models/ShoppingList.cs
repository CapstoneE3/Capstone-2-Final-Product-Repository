using System;
using System.Collections.Generic;

#nullable disable

namespace PantryBackEnd.Models
{
    public partial class ShoppingList
    {
        public string ItemId { get; set; }
        public Guid AccId { get; set; }
        public int Count { get; set; }

        public virtual Account Acc { get; set; }
        public virtual Product Item { get; set; }
    }
}
