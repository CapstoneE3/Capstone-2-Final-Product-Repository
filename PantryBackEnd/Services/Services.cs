using System.Collections.Generic;
using PantryBackEnd.Models;
namespace PantryBackEnd.Services
{
    public static class Services
    {
        public static bool FindDuplicate(ICollection<InventoryList> list,ProductDt product)
        {
            bool duplicate = false;
            foreach(InventoryList a in list)
            {
                if(product.productID.Equals(a.ItemId))
                {
                    duplicate = true;
                    break;
                }
            }
            return duplicate;

        }
    }
}