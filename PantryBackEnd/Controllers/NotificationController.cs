using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PantryBackEnd.Models;
using PantryBackEnd.Repositories;
using PantryBackEnd.JwtGenerator;
using WebPush;
using Newtonsoft.Json;
namespace PantryBackEnd.Controllers
{
    public class NotificationController : ControllerBase
    {
        private INotification store;
        private IUserRepo userRepo;
        private JwtService service;

        public NotificationController(JwtService service, IUserRepo userRepo, INotification store)
        {
            this.service = service;
            this.userRepo = userRepo;
            this.store = store;
        }

        [Route("api/subcriptions")]
        [HttpPost]
        public async Task<ActionResult> StoreSubscription(SubscriptionFrontEnd subs)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                Account user = userRepo.GetByID(userId);
                Subscription subDbEntry = new Subscription
                {
                    AccId = userId,
                    SubEndpoint = subs.Endpoint,
                    ExpNotif = subs.expiry,
                    Key = subs.key,
                    Audh = subs.auth
                };
                await store.StoreSubscription(subDbEntry);
                return NoContent();
            }
            catch (Exception)
            {
                return Unauthorized(new { message = "Unauthorized" });
            }
        }
        [Route("api/DeleteSubcriptions")]
        [HttpDelete]
        public async Task<ActionResult> DeleteSubs(SubscriptionFrontEnd subs)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                Account user = userRepo.GetByID(userId);
                await store.DeleteSub(userId);
                return NoContent();
            }
            catch (Exception)
            {
                return Unauthorized(new { message = "Unauthorized" });
            }
        }

        [Route("api/Test")]
        [HttpGet]
        public async Task<ActionResult> TestNotification()
        {
            try
            {

                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer); ;
                WebPushClient webpush = new WebPushClient();
                VapidDt details = store.GetVapidDt();
                webpush.SetVapidDetails(details.subjecy, details.publicKey, details.privateKey);
                Subscription subscriptionData = store.GetSubscription(userId);

                PushSubscription push = new PushSubscription(subscriptionData.SubEndpoint, subscriptionData.Key, subscriptionData.Audh);
                string payload = "Your items are expiring ";
                await webpush.SendNotificationAsync(push, payload);
                return Ok();
            }
            catch (Exception)
            {
                return Ok();
            }

        }
    }
}