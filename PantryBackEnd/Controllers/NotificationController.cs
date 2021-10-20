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
                Subscription subDbEntry = new Subscription
                {
                    AccId = userId,
                    SubEndpoint = subs.Endpoint,
                    Key = subs.key,
                    Audh = subs.auth
                };
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
        [Route("api/DeleteSubcriptions")]
        [HttpDelete]
        public async Task<ActionResult> DeleteSubs([FromBody] SubscriptionFrontEnd subs)
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