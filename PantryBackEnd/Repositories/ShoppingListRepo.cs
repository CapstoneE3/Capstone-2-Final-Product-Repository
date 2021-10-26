using System.Threading.Tasks;
using PantryBackEnd.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
namespace PantryBackEnd.Repositories
{
    public class ShoppingListRepo : IShoppingList
    {
        private pantryContext context;

        public ShoppingListRepo(pantryContext context)
        {
            this.context = context;
        }

        public Task addShoppingItems(ShoppingList items)
        {
            context.ShoppingLists.Add(items);
            context.SaveChanges();
            return Task.CompletedTask;
        }
        public Task AddShoppingList(List<ShoppingList> items)
        {

            context.ShoppingLists.AddRange(items);
            context.SaveChanges();
            return Task.CompletedTask;
        }
        public int GetItemCount(ShoppingList items)
        {
            try
            {

                return context.ShoppingLists.Where(a => a.Item.ItemId.Equals(items.ItemId)).Where(d => d.AccId == items.AccId).Select(c => c.Count).Single();
            }
            catch (InvalidOperationException)
            {
                return 0;
            }
        }
        public Task updateItems(ShoppingList item)
        {
            context.ShoppingLists.Update(item);
            context.SaveChanges();
            return Task.CompletedTask;
        }
        public Task DeleteShoppingItem(string itemID, Guid id)
        {
            context.ShoppingLists.Remove(context.ShoppingLists.Where(a => a.AccId == id && a.ItemId.Equals(itemID)).Single());
            context.SaveChanges();
            return Task.CompletedTask;
        }
        public Dictionary<string, List<ShoppingItemsFormat>> getShoppingList(Guid id)
        {
            List<ShoppingItemsFormat> items = new List<ShoppingItemsFormat>();
            Dictionary<string, List<ShoppingItemsFormat>> diction = new Dictionary<string, List<ShoppingItemsFormat>>();
            foreach (ShoppingList a in context.ShoppingLists.Where(a => a.AccId == id).Include(c => c.Item).ToList())
            {
                ShoppingItemsFormat shopItem = new ShoppingItemsFormat
                {
                    ItemId = a.ItemId,
                    name = a.Item.Name,
                    price = (decimal)a.Item.Price,
                    Count = a.Count,
                    quantity = a.Item.Quantity,
                    category = a.Item.Category
                };
                if (diction.ContainsKey(a.Item.Category))
                {
                    diction[a.Item.Category].Add(shopItem);
                }
                else
                {
                    diction.Add(a.Item.Category, new List<ShoppingItemsFormat>());
                    diction[a.Item.Category].Add(shopItem);
                }
            }
            return diction;
        }
        public Account getUserShoppingList(Guid id)
        {
            return context.Accounts.Where(a => a.AccId == id).Include(b => b.ShoppingLists).Single();
        }
        public void UpdateShop()
        {
            context.SaveChanges();
        }

    }
}