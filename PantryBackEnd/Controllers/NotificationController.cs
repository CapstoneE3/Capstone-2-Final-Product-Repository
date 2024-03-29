using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PantryBackEnd.Models;
using PantryBackEnd.Repositories;
using PantryBackEnd.JwtGenerator;
using WebPush;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
namespace PantryBackEnd.Controllers
{
    public class NotificationController : ControllerBase
    {
        ILogger<NotificationController> logger;
        private INotification store;
        private IUserRepo userRepo;
        private JwtService service;

        public NotificationController(JwtService service, IUserRepo userRepo, INotification store, ILogger<NotificationController> logger)
        {
            this.service = service;
            this.userRepo = userRepo;
            this.store = store;
            this.logger = logger;
        }

        [Route("api/subcriptions")]
        [HttpPost]
        public async Task<ActionResult<Subscription>> StoreSubscription([FromBody] SubscriptionFrontEnd subs)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                Account user = userRepo.GetByID(userId);
                Subscription subDbEntry = new Subscription();
                subDbEntry.AccId = userId;
                subDbEntry.SubEndpoint = subs.endpoint;
                foreach (KeyValuePair<string, string> c in subs.keys)
                {
                    if (c.Key.Equals("auth"))
                    {
                        subDbEntry.Audh = c.Value;
                    }
                    else
                    {
                        subDbEntry.Key = c.Value;
                    }
                }
                await store.StoreSubscription(subDbEntry);
                Subscription a = store.GetSubscription(userId);
                return Ok(a);
            }
            catch (Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException)
            {
                Response.Cookies.Delete("jwt");
                return Unauthorized(new { message = "Expired" });
            }
            catch (Exception)
            {
                return Unauthorized(new { message = "Unauthorized" });
            }
        }
        [Route("api/CheckSubscription")]
        [HttpPost]
        public ActionResult<CheckTHemSubs> CheckNotification([FromBody] SubscriptionFrontEnd subs)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                Account user = userRepo.GetByID(userId);
                Subscription subDbEntry = new Subscription();
                subDbEntry.AccId = userId;
                subDbEntry.SubEndpoint = subs.endpoint;
                foreach (KeyValuePair<string, string> c in subs.keys)
                {
                    if (c.Key.Equals("auth"))
                    {
                        subDbEntry.Audh = c.Value;
                    }
                    else
                    {
                        subDbEntry.Key = c.Value;
                    }
                }
                CheckTHemSubs check = new CheckTHemSubs();
                check.Subscribed = store.CheckSubs(subDbEntry);
                return Ok(check);
            }
            catch (Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException)
            {
                Response.Cookies.Delete("jwt");
                return Unauthorized(new { message = "Expired" });
            }
            catch (Exception)
            {
                return Unauthorized(new { message = "Unauthorized" });
            }
        }
        [Route("api/replaceSubscription")]
        [HttpPost]
        public async Task<ActionResult<Subscription>> ReplaceSub([FromBody] SubscriptionFrontEnd subs)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                Account user = userRepo.GetByID(userId);
                Subscription subDbEntry = new Subscription();
                subDbEntry.AccId = userId;
                subDbEntry.SubEndpoint = subs.endpoint;
                foreach (KeyValuePair<string, string> c in subs.keys)
                {
                    if (c.Key.Equals("auth"))
                    {
                        subDbEntry.Audh = c.Value;
                    }
                    else
                    {
                        subDbEntry.Key = c.Value;
                    }
                }
                await store.UpdateSub(subDbEntry);
                Subscription a = store.GetSubscription(userId);
                return Ok(a);
            }
            catch (Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException)
            {
                Response.Cookies.Delete("jwt");
                return Unauthorized(new { message = "Expired" });
            }
            catch (Exception)
            {
                return Unauthorized(new { message = "Unauthorized" });
            }
        }
        [Route("TestingAzure")]
        [HttpGet]
        public ActionResult Test()
        {
            Console.WriteLine("Hello");
            Console.WriteLine(JsonConvert.SerializeObject(store.GetVapidDt()));
            logger.LogInformation(JsonConvert.SerializeObject(store.GetVapidDt()));
            return Ok();

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
            catch (Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException)
            {
                Response.Cookies.Delete("jwt");
                return Unauthorized(new { message = "Expired" });
            }
            catch (Exception)
            {
                return Ok();
            }

        }
    }
}