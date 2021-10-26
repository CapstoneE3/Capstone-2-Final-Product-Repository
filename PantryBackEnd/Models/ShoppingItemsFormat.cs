using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PantryBackEnd.Models
{
    public class ShoppingItemsFormat
    {
        public string ItemId { get; set; }
        public int Count { get; set; }

        public decimal price { get; set; }
        public string name { get; set; }
        public string quantity { get; set; }
        public string category { get; set; }

    }
}