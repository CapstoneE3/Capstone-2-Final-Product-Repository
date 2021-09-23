using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PantryBackEnd.Models;
namespace PantryBackEnd.Repositories
{
    public interface INotification
    {
        Task StoreSubscription(Subscription subs, Guid id);

        Task DeleteSub(Guid id);
    }
}