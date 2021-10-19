using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PantryBackEnd.Models;
using WebPush;
using Newtonsoft.Json;
using System.Globalization;
using PantryBackEnd.Repositories;
namespace PantryBackEnd.Notification
{
    public class SendNotification
    {
        INotification notification;

        public SendNotification(INotification notification1)
        {
            this.notification = notification1;
        }

        public List<ProductDt> getNotifactionTIme(List<ProductDt> list)
        {
            foreach (var item in list)
            {
                item.NotificationTime = item.exp.Subtract(new TimeSpan(4, 0, 0, 0, 0));
            }
            return list;
        }
        public void PreparingNotification(Guid id, List<ProductDt> list)
        {
            List<SubscriptionData> why = new List<SubscriptionData>();
            List<DateTime> not = new List<DateTime>();
            foreach (var item in list)
            {
                if (!not.Contains(item.NotificationTime))
                {
                    not.Add(item.NotificationTime);
                }
            }
            foreach (var item in not)
            {
                SubscriptionData data = new SubscriptionData();
                data.title = "Items expiring soon";
                data.expiry = item;
                foreach (var i in list)
                {
                    if (i.NotificationTime == item)
                    {
                        data.body.Add(notification.getProductName(i.productID) + ",");
                    }
                }
                why.Add(data);
            }
            sendThem(why, id);


        }
        private async void sendThem(List<SubscriptionData> list, Guid id)
        {
            WebPushClient webpush = new WebPushClient();
            VapidDt details = notification.GetVapidDt();
            webpush.SetVapidDetails(details.subjecy, details.publicKey, details.privateKey);
            List<string> names = new List<string>();
            foreach (SubscriptionData x in list)
            {
                Subscription subscriptionData = notification.GetSubscription(id);
                PushSubscription push = new PushSubscription(subscriptionData.SubEndpoint, subscriptionData.Key, subscriptionData.Audh);
                string data = JsonConvert.SerializeObject(x);
                string payload = data;
                await webpush.SendNotificationAsync(push, payload);
            }
        }
    }
}