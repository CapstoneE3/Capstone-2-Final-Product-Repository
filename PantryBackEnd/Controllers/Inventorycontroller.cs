using Microsoft.AspNetCore.Mvc;
using PantryBackEnd.JwtGenerator;
using PantryBackEnd.Models;
using System;
using PantryBackEnd.Repositories;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using PantryBackEnd.Notification;
namespace PantryBackEnd.Controllers
{
    public class Inventorycontroller : ControllerBase
    {

        private Guid guid = Guid.NewGuid();
        private JwtService service;
        private IInventoryRepo InvRepo;
        private IUserRepo userRepo;
        private IProduct productRep;
        private SendNotification notification;
        ILogger<Inventorycontroller> logger;
        public Inventorycontroller(IInventoryRepo context, JwtService service, IUserRepo userRepo, IProduct productRep, SendNotification notification, ILogger<Inventorycontroller> logger)
        {
            this.service = service;
            this.InvRepo = context;
            this.userRepo = userRepo;
            this.productRep = productRep;
            this.notification = notification;
            this.logger = logger;
        }
        /*
        Add a single product to the database 
         */
        [Route("api/SingleProduct")]
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductDt dt)
        {
            try
            {
                // check if the request have a valid cookie
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
                // get inventory repo
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
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
                products.items = notification.getNotifactionTIme(products.items);
                logger.LogInformation(products.items.ToString());
                logger.LogInformation("Hello Im line 123");
                if (user.InventoryLists == null || user.InventoryLists.Count == 0)
                {
                    logger.LogInformation("Hello Im line 125");
                    await Task.Run(() =>
                    {
                        foreach (ProductDt a in products.items)
                        {
                            InventoryList inv = new InventoryList { AccId = userId, ItemId = a.productID, ExpDate = a.exp, Count = a.count, NotificationTime = a.NotificationTime };
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
                        logger.LogInformation("Hello Im line 143");
                        foreach (ProductDt a in products.items)
                        {
                            count = Services.Services.getCount(user.InventoryLists, a);
                            logger.LogInformation(a + " " + "Product count " + a.count);
                            if (count == 0)
                            {
                                logger.LogInformation("Hello Im line new product");
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
                                logger.LogInformation("Hello Im line Update");

                                InvRepo.updateItem();
                            }
                        }
                    });
                    notification.PreparingNotification(userId, products.items);
                }
            }
            catch (Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException)
            {
                Response.Cookies.Delete("jwt");
                return Unauthorized(new { message = "Expired" });
            }
            catch (Exception)
            {
                logger.LogInformation("Hello Im line 181");
                return BadRequest();
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