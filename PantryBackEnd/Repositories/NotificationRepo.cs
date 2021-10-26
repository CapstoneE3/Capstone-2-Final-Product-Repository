using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PantryBackEnd.Models;
using Microsoft.Extensions.Configuration;
namespace PantryBackEnd.Repositories
{
    public class NotificationRepo : INotification
    {
        private pantryContext context;
        private readonly VapidDt vapidDetails;

        public NotificationRepo(pantryContext context, IConfiguration configuration)
        {
            this.context = context;
            var vapidSubject = configuration.GetValue<string>("Vapid:Subject");
            var vapidPublicKey = configuration.GetValue<string>("Vapid:PublicKey");
            var vapidPrivateKey = configuration.GetValue<string>("Vapid:PrivateKey");

            vapidDetails = new VapidDt(vapidPrivateKey, vapidPublicKey, vapidSubject);
        }

        public VapidDt GetVapidDt()
        {
            return vapidDetails;
        }
        public Task StoreSubscription(Subscription subs)
        {
            context.Subscriptions.Add(subs);
            context.SaveChanges();
            return Task.CompletedTask;
        }
        public async Task UpdateSub(Subscription subs)
        {

            context.Subscriptions.Update(subs);
            await context.SaveChangesAsync();
            Task.WaitAll();
        }
        public List<InventoryList> GetInventoryList()
        {
            return context.InventoryLists.Where(a => a.ExpDate >= DateTime.Today).ToList();
        }

        public string getProductName(string itemId)
        {
            return context.Products.Single(a => a.ItemId.Equals(itemId)).Name;
        }
        public Subscription GetSubscription(Guid id)
        {
            return context.Subscriptions.Where(a => a.AccId == id).Single();
        }
        public bool CheckSubs(Subscription subs)
        {
            return context.Subscriptions.Any(a => a.SubEndpoint == subs.SubEndpoint);
        }


    }
}