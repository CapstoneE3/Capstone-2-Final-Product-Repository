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

        Task UpdateSub(Subscription subs);

        List<InventoryList> GetInventoryList();

        string getProductName(string itemId);

        Subscription GetSubscription(Guid id);

        bool CheckSubs(Subscription subs);

        VapidDt GetVapidDt();
    }
}