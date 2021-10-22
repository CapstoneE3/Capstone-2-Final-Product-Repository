using System.Threading.Tasks;
using PantryBackEnd.Models;
using System.Collections.Generic;
using System;
namespace PantryBackEnd.Repositories
{
    public interface IShoppingList
    {
        Task addShoppingItems(ShoppingList items);
        List<ShoppingItemsFormat> getShoppingList(Guid id);
        Task DeleteShoppingItem(string itemID, Guid id);

        Task AddShoppingList(List<ShoppingList> items);
    }
}