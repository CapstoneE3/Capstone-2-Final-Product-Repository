using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PantryBackEnd.Models;
using PantryBackEnd.Repositories;
using PantryBackEnd.JwtGenerator;
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
        public async Task<ActionResult> StoreSubscription([FromBody] Subscription subs)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                Account user = userRepo.GetByID(userId);
                await store.StoreSubscription(subs, userId);
                return NoContent();
            }
            catch (Exception)
            {
                return Unauthorized(new { message = "Unauthorized" });
            }
        }
        [Route("api/DeleteSubcriptions")]
        [HttpDelete]
        public async Task<ActionResult> DeleteSubs([FromBody] Subscription subs)
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
    }
}