using PantryBackEnd.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PantryBackEnd.Repositories
{

    public class InventoryRepo : IInventoryRepo
    {
        private pantryContext context;
        public InventoryRepo(pantryContext context)
        {
            this.context = context;
        }
        public void AddProduct(InventoryList product)
        {
            context.InventoryLists.Add(product);
            context.SaveChanges();
        }
        public void updateItem(InventoryList product)
        {
            context.InventoryLists.Update(product);
            context.SaveChanges();
        }
        public Dictionary<string, object> GetInventoryList(Guid acc_id)
        {
            Dictionary<string, object> invList = new Dictionary<string, object>();
            string[] category = {"Kids & Lunch Box","Entertaining At Home","Bakery","Fruit & Vegetables",
                                "Meat & Seafood","From The Deli","Dairy, Eggs & Meals","Conveniece Meals",
                                "Pantry","Frozen","Drinks","International Foods","Household","Health & Beauty",
                                "Baby","Pet","Liquor","Tobacco"};
            foreach (string i in category)
            {
                Product prods = context.Products.Where(a => a.Category.Equals(i)).Include(b => b.InventoryLists.Where(c => c.AccId == acc_id)).FirstOrDefault();
                try
                {
                    invList.Add(i, prods.InventoryLists);
                }
                catch (NullReferenceException)
                {
                    invList.Add(i, new List<InventoryList>());
                }
            }
            return invList;
        }
        public bool sameItemExist(Guid acc_ID, string itemID, DateTime exp)
        {
            try
            {
                InventoryList item = context.InventoryLists.Where(b => b.AccId.Equals(acc_ID) && b.ItemId.Equals(itemID) && b.ExpDate.Equals(exp)).Single();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}