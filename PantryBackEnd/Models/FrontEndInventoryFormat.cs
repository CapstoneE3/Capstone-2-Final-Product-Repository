using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PantryBackEnd.Models
{
    public class FrontEndInventoryFormat
    {
        public string itemID { get; set; }
        public string name { get; set; }
        public string quantity { get; set; }
        public decimal price { get; set; }
        public string photo { get; set; }

        public List<ExpDateAndPrice> Expiry_Count = new List<ExpDateAndPrice>();


    }
}