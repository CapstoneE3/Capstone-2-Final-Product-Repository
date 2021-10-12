using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PantryBackEnd.Models
{
    public class TillShoppingItems
    {
        public List<ProductDt> items;
        public Guid AccountId;
    }
}