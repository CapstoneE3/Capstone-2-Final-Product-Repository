using System.Threading.Tasks;
using PantryBackEnd.Models;
using System.Collections.Generic;
using System;
namespace PantryBackEnd.Repositories
{
    public interface IShoppingList
    {
        Task addShoppingItems(ShoppingList items);
        Dictionary<string, List<ShoppingItemsFormat>> getShoppingList(Guid id);
        Task DeleteShoppingItem(string itemID, Guid id);

        Task AddShoppingList(List<ShoppingList> items);
        int GetItemCount(ShoppingList items);
        Task updateItems(ShoppingList item);
        Account getUserShoppingList(Guid id);
        void UpdateShop();
    }
}