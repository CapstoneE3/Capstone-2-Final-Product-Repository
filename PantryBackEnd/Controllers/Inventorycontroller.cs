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

        }
        public int getCount(ICollection<InventoryList> list, ProductDt product)
        {
            foreach (InventoryList a in list)
            {
                if (product.productID.Equals(a.ItemId) && DateTime.Equals(a.ExpDate.Date,product.exp.Date))
                {
                    a.Count += product.count;
                    return (int)a.Count;
                }
            }
            return 0;
        }
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
                int count = getCount(user.InventoryLists, dt);
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
        public async Task<IActionResult> AddProductFromQR(TillShoppingItems products)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                Account user = userRepo.GetAccountWithInv(products.uid);
                List<InventoryList> list = new List<InventoryList>();
                products.items = notification.getNotifactionTIme(products.items); ;
                if (user.InventoryLists == null || user.InventoryLists.Count == 0)
                {
                    await Task.Run(() =>
                    {
                        foreach (ProductDt a in products.items)
                        {
                            InventoryList inv = new InventoryList { AccId = products.uid, ItemId = a.productID, ExpDate = a.exp, Count = a.count, NotificationTime = a.NotificationTime };
                            list.Add(inv);
                        }
                    });
                    InvRepo.AddTIllProduct(list);

                    notification.PreparingNotification(userId, products.items);
                }
                else
                {
                    int count = 6;
                    await Task.Run(() =>
                    {
                        foreach (ProductDt a in products.items)
                        {
                            count = getCount(user.InventoryLists, a);
                            if (count == 0)
                            {
                                InventoryList item = new InventoryList
                                {
                                    ItemId = a.productID,
                                    ExpDate = a.exp,
                                    AccId = products.uid,
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