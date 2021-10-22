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
        public Task DeleteShoppingItem(string itemID, Guid id)
        {
            context.ShoppingLists.Remove(context.ShoppingLists.Where(a => a.AccId == id && a.ItemId.Equals(itemID)).Single());
            context.SaveChanges();
            return Task.CompletedTask;
        }
        public List<ShoppingItemsFormat> getShoppingList(Guid id)
        {
            List<ShoppingItemsFormat> items = new List<ShoppingItemsFormat>();
            foreach (ShoppingList a in context.ShoppingLists.Where(a => a.AccId == id).Include(c => c.Item).ToList())
            {
                ShoppingItemsFormat shopItem = new ShoppingItemsFormat
                {
                    ItemId = a.ItemId,
                    name = a.Item.Name,
                    price = (decimal)a.Item.Price,
                    Count = a.Count
                };
                items.Add(shopItem);
            }
            return items;
        }

    }
}