using System.Collections.Generic;
using PantryBackEnd.Models;
namespace PantryBackEnd.Services
{
    public static class Services
    {
        public static bool FindDuplicate(ICollection<InventoryList> list, ProductDt product)
        {
            bool duplicate = false;
            foreach (InventoryList a in list)
            {
                if (product.productID.Equals(a.ItemId) && product.exp.Equals(a.ExpDate))
                {
                    duplicate = true;
                    break;
                }
            }
            return duplicate;

        }
        public static int getCount(ICollection<InventoryList> list, ProductDt product)
        {
            foreach (InventoryList a in list)
            {
                if (product.productID.Equals(a.ItemId) && product.exp.Equals(a.ExpDate))
                {
                    return (int)a.Count;
                }
            }
            return 0;
        }
    }
}