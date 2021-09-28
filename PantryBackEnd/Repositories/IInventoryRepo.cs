using PantryBackEnd.Models;
using System.Collections.Generic;
using System;

namespace PantryBackEnd.Repositories
{
    public interface IInventoryRepo
    {
        void AddProduct(InventoryList product);
        Dictionary <string, object> GetInventoryList(Guid acc_id);
        void removeProduct(Guid acc_id, string item_id);
        

        
        void updateItem();
    }
}