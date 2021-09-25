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
        public void updateItem()
        {
            context.SaveChanges();
        }
        public Dictionary<string, object> GetInventoryList(Guid acc_id)
        {
            int listCount = 0;
            List<FrontEndInventoryFormat> formats;
            Dictionary<string, object> invList = new Dictionary<string, object>();
            string[] category = {"Kids & Lunch Box","Entertaining At Home","Bakery","Fruit & Vegetables",
                                "Meat & Seafood","From The Deli","Dairy, Eggs & Meals","Conveniece Meals",
                                "Pantry","Frozen","Drinks","International Foods","Household","Health & Beauty",
                                "Baby","Pet","Liquor","Tobacco","Sugar & Sweeteners"};
            foreach (string i in category)
            {
                List<Product> prods = context.Products.Where(a => a.Category.Equals(i)).Include(b => b.InventoryLists.Where(c => c.AccId == acc_id)).ToList();
                if (prods.Count > 0)
                {
                    formats = new List<FrontEndInventoryFormat>();
                    foreach (Product c in prods)
                    {

                        if (c.InventoryLists.Count != 0)
                        {
                            listCount = 0;
                            FrontEndInventoryFormat b = new FrontEndInventoryFormat();
                            foreach (InventoryList a in c.InventoryLists)
                            {
                                ExpDateAndPrice eap = new ExpDateAndPrice
                                {
                                    expDate = a.ExpDate,
                                    count = (int)a.Count
                                };
                                if (listCount == 0)
                                {
                                    b.itemID = a.ItemId;
                                    b.name = c.Name;
                                    b.price = (decimal)c.Price;
                                    b.quantity = c.Quantity;
                                    b.Expiry_Count.Add(eap);
                                }
                                else
                                {
                                    b.Expiry_Count.Add(eap);
                                }
                                listCount++;


                            }
                            formats.Add(b);
                        }
                    }
                    invList.Add(i, formats);
                }
                else
                {
                    invList.Add(i, new List<FrontEndInventoryFormat>());
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