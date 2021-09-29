using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NCrontab;
using PantryBackEnd.Repositories;
using PantryBackEnd.Models;
using WebPush;
using Newtonsoft.Json;
namespace PantryBackEnd
{
    public class PushNotfication : BackgroundService
    {
        int delay = 5000;
        ILogger<PushNotfication> logger;
        private CrontabSchedule crontab;
        private DateTime NexRun;
        private const string schedule = "0 0 12 * * *";

        private INotification notification;

        public PushNotfication(IServiceScopeFactory provider, ILogger<PushNotfication> logger)
        {
            crontab = CrontabSchedule.Parse("0 12 * * * ");
            NexRun = crontab.GetNextOccurrence(DateTime.Now);
            this.notification = provider.CreateScope().ServiceProvider.GetRequiredService<INotification>();
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            do
            {
                var now = DateTime.Now;
                if (now > NexRun)
                {
                    doWork();
                    Console.WriteLine(DateTime.Now);
                    NexRun = crontab.GetNextOccurrence(DateTime.Now);
                    delay = 1000 * 60 * 60 * 24;
                }
                logger.LogInformation(now + " " + NexRun + " " + JsonConvert.SerializeObject(notification.GetVapidDt()));
                await Task.Delay(delay, stoppingToken);
            } while (!stoppingToken.IsCancellationRequested);
        }

        public void doWork()
        {
            SubscriptionData data;
            List<object> listData = new List<object>();
            List<InventoryList> list = notification.GetInventoryList();
            Dictionary<Guid, object> categoriseUser = new Dictionary<Guid, object>();
            int count = 0;

            Guid id = Guid.Empty;
            foreach (InventoryList a in list)
            {
                if (count == 0)
                {
                    id = a.AccId;
                    data = new SubscriptionData { name = notification.getProductName(a.ItemId), expiry = a.ExpDate };
                    listData.Add(data);
                    count++;
                }
                else
                {
                    if (id != a.AccId)
                    {
                        categoriseUser.Add(id, listData);
                        id = a.AccId;
                        listData = new List<object>();
                        data = new SubscriptionData { name = notification.getProductName(a.ItemId), expiry = a.ExpDate };
                        listData.Add(data);
                    }
                    else
                    {
                        data = new SubscriptionData { name = notification.getProductName(a.ItemId), expiry = a.ExpDate };
                        listData.Add(data);
                    }
                }
            }
            push(categoriseUser);

        }
        public async void push(Dictionary<Guid, object> list)
        {
            WebPushClient webpush = new WebPushClient();
            VapidDt details = notification.GetVapidDt();
            webpush.SetVapidDetails(details.subjecy, details.publicKey, details.privateKey);
            foreach (KeyValuePair<Guid, object> a in list)
            {
                Subscription subscriptionData = notification.GetSubscription(a.Key);

                PushSubscription push = new PushSubscription(subscriptionData.SubEndpoint, subscriptionData.Key, subscriptionData.Audh);
                string data = JsonConvert.SerializeObject(a.Value);
                string payload = "Your items are expiring " + data;
                await webpush.SendNotificationAsync(push, payload);
            }
        }
    }
}
