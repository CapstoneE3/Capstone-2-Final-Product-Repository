using System.Collections.Generic;
using PantryBackEnd.Models;
namespace PantryBackEnd.Services
{
    public static class Services
    {
        public static int getCount(ICollection<InventoryList> list, ProductDt product)
        {
            foreach (InventoryList a in list)
            {
                if (product.productID.Equals(a.ItemId) && product.exp.Equals(a.ExpDate))
                {
                    a.Count += product.count;
                    return (int)a.Count;
                }
            }
            return 0;
        }
    }
}