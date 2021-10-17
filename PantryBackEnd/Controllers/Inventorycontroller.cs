using Microsoft.AspNetCore.Mvc;
using PantryBackEnd.JwtGenerator;
using PantryBackEnd.Models;
using System;
using PantryBackEnd.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;
using PantryBackEnd.Services;
using Microsoft.AspNetCore.Http;
using PantryBackEnd.Notification;
namespace PantryBackEnd.Controllers
{
    public class Inventorycontroller : ControllerBase
    {

        private Guid guid = Guid.NewGuid();
        JwtService service;
        private IInventoryRepo InvRepo;
        private IUserRepo userRepo;
        private IProduct productRep;
        private SendNotification notification;
        public Inventorycontroller(IInventoryRepo context, JwtService service, IUserRepo userRepo, IProduct productRep, SendNotification notification)
        {
            this.service = service;
            this.InvRepo = context;
            this.userRepo = userRepo;
            this.productRep = productRep;
            this.notification = notification;
        }

        [Route("api/SingleProduct")]
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductDt dt)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                Account user = userRepo.GetAccountWithInv(userId);
                int count = Services.Services.getCount(user.InventoryLists, dt);
                if (count == 0)
                {
                    InventoryList item = new InventoryList
                    {
                        ItemId = dt.productID,
                        ExpDate = dt.exp,
                        AccId = userId,
                        Count = dt.count,
                        NotificationTime = dt.NotificationTime
                    };
                    await Task.Run(() =>
                    {
                        InvRepo.AddProduct(item);
                    }
                    );
                }
                else
                {
                    await Task.Run(() =>
                    {
                        InvRepo.updateItem();
                    }
                    );
                }

                return Ok();

            }
            catch (Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException)
            {
                Response.Cookies.Delete("jwt");
                return Unauthorized(new { message = "Expired" });
            }
            catch (Exception)
            {
                return Unauthorized();
            }


        }
        [Route("api/GetInventoryList")]
        [HttpGet]
        public ActionResult<Dictionary<string, object>> GetInventoryLists()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                Account user = userRepo.GetByID(userId);
                var list = InvRepo.GetInventoryList(userId);
                return Ok(list);
            }
            catch (Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException)
            {
                Response.Cookies.Delete("jwt");
                return Unauthorized(new { message = "Expired" });
            }
            catch (Exception)
            {
                return Unauthorized();
            }

        }

        [Route("api/QRProducts")]
        [HttpPost]
        public async Task<IActionResult> AddProductFromQR([FromBody] TillShoppingItems products)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                Account user = userRepo.GetAccountWithInv(userId);
                List<InventoryList> list = new List<InventoryList>();
                if (user.InventoryLists == null)
                {
                    await Task.Run(() =>
                    {
                        foreach (ProductDt a in products.items)
                        {
                            InventoryList inv = new InventoryList { AccId = userId, ItemId = a.productID, NotificationTime = a.exp, Count = a.count };
                            list.Add(inv);
                        }
                    });
                    InvRepo.AddTIllProduct(list);
                    notification.PreparingNotification(userId, products.items);
                }
                else
                {
                    int count = 0;
                    await Task.Run(() =>
                    {
                        foreach (ProductDt a in products.items)
                        {
                            count = Services.Services.getCount(user.InventoryLists, a);
                            if (count == 0)
                            {
                                InventoryList item = new InventoryList
                                {
                                    ItemId = a.productID,
                                    ExpDate = a.exp,
                                    AccId = userId,
                                    Count = a.count,
                                    NotificationTime = a.NotificationTime
                                };
                                InvRepo.AddProduct(item);
                            }
                            else
                            {
                                InvRepo.updateItem();
                            }
                        }
                        notification.PreparingNotification(userId, products.items);
                    }
                    );
                }
            }
            catch (Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException)
            {
                Response.Cookies.Delete("jwt");
                return Unauthorized(new { message = "Expired" });
            }
            catch (Exception)
            {
                return Unauthorized();
            }
            return Ok();
        }

        [Route("api/removeInventoryItem")]
        [HttpDelete]
        public ActionResult removeInventoryItem([FromBody] ProductDt dt)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                Account user = userRepo.GetByID(userId);

                string msg = InvRepo.removeProduct(userId, dt.productID, dt.count, dt.exp);


                return Ok(new { message = "Success" });

            }
            catch (Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException)
            {
                Response.Cookies.Delete("jwt");
                return Unauthorized(new { message = "Expired" });
            }
            catch (Exception)
            {
                return Unauthorized(new { message = "Failed" });
            }

        }

        [Route("api/getAPIRecipe")]
        [HttpGet]
        public async Task<ActionResult> getAPIRecipe()
        {

            await InvRepo.randomRecipe();
            return Ok(new { message = "Success" });


        }
    }
}