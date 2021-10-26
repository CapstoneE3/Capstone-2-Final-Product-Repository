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
        public Inventorycontroller(IInventoryRepo context, JwtService service, IUserRepo userRepo, IProduct productRep, SendNotification notification, ILogger<Inventorycontroller> logger)
        {
            this.service = service;
            this.InvRepo = context;
            this.userRepo = userRepo;
            this.productRep = productRep;
            this.notification = notification;

        }
        public int getCount(ICollection<InventoryList> list, ProductDt product)
        {
            foreach (InventoryList a in list)
            {
                if (product.productID.Equals(a.ItemId) && DateTime.Equals(a.ExpDate.Date, product.exp.Date))
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
        public async Task<IActionResult> AddProductFromQR([FromBody] TillShoppingItems products)
        {
            try
            {
                if (products == null)
                {
                    return BadRequest(new { message = "ITs null" });
                }
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                int count = 0;
                Account user = userRepo.GetAccountWithInv(products.AccountId);
                List<InventoryList> list = new List<InventoryList>();
                products.items = notification.getNotifactionTIme(products.items); ;
                if (user.InventoryLists == null)
                {
                    await Task.Run(() =>
                    {
                        foreach (ProductDt a in products.items)
                        {
                            InventoryList inv = new InventoryList { AccId = products.AccountId, ItemId = a.productID, ExpDate = a.exp, Count = a.count, NotificationTime = a.NotificationTime };
                            list.Add(inv);
                        }
                    });
                    InvRepo.AddTIllProduct(list);

                    notification.PreparingNotification(products.AccountId, products.items);
                }

                else
                {

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
                                    AccId = products.AccountId,
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
                    notification.PreparingNotification(products.AccountId, products.items);
                }
            }
            catch (Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException)
            {
                Response.Cookies.Delete("jwt");
                return Unauthorized(new { message = "Expired" });
            }
            catch (Exception e)
            {

                return BadRequest(new { message = e });
            }
            return Ok();
        }

        [Route("api/removeInventoryItem")]
        [HttpDelete]
        public async Task<ActionResult> removeInventoryItem([FromBody] RemoveProduct dt)
        {
            try
            {
                if (dt == null)
                {
                    return Ok(new { message = "object is null" });
                }
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);


                string msg = await InvRepo.removeProduct(userId, dt.productID, dt.count, dt.exp);

                return Ok(new { message = "Success" });

            }
            catch (Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException)
            {
                Response.Cookies.Delete("jwt");
                return Unauthorized(new { message = "Expired" });
            }
            catch (Exception e)
            {
                return Unauthorized(new { message = "Failed" + e });
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