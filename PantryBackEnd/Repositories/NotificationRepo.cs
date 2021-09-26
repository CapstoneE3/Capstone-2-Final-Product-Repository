using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PantryBackEnd.Models;
namespace PantryBackEnd.Repositories
{
    public class NotificationRepo : INotification
    {
        pantryContext context;

        public NotificationRepo(pantryContext context)
        {
            this.context = context;
        }

        public Task StoreSubscription(Subscription subs, Guid id)
        {
            return Task.Run(() =>
            {
                Account ac = context.Accounts.FirstOrDefault(a => a.AccId == id);
                //modify ac and then update
                context.Accounts.Update(ac);
                context.SaveChangesAsync();
            });
        }
        public Task DeleteSub(Guid id)
        {
            return Task.Run(() =>
            {

            }
            );
        }
    }
}