using System.Collections.Generic;
using PantryBackEnd.Models;
using System;
namespace PantryBackEnd.Services
{
    public static class Services
    {
        public static int getCount(ICollection<InventoryList> list, ProductDt product)
        {
            foreach (InventoryList a in list)
            {
                if (product.productID.Equals(a.ItemId) && DateTime.Equals(a.ExpDate.Date, product.exp.Date))
                {
                    a.Count += product.count;
                    return (int)a.Count;
                }
            }
            return 0;
        }

    }
}