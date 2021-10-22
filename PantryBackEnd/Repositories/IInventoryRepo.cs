using PantryBackEnd.Models;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
namespace PantryBackEnd.Repositories
{
    public interface IInventoryRepo
    {
        void AddProduct(InventoryList product);
        Dictionary<string, object> GetInventoryList(Guid acc_id);
        Task<string> removeProduct(Guid acc_id, string item_id, int count, DateTime exp);
        Task randomRecipe();

        void AddTIllProduct(List<InventoryList> product);


        void updateItem();
    }
}