using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PantryBackEnd.Models;
namespace PantryBackEnd.Repositories
{
    public interface INotification
    {
        Task StoreSubscription(Subscription subs);

        Task DeleteSub(Guid id);

        List<InventoryList> GetInventoryList();

        string getProductName(string itemId);

        Subscription GetSubscription(Guid id);

        VapidDt GetVapidDt();
    }
}