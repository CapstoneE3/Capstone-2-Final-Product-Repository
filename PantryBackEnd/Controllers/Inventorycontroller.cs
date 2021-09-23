using Microsoft.AspNetCore.Mvc;
using PantryBackEnd.JwtGenerator;
using PantryBackEnd.Models;
using System;
using PantryBackEnd.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;
using PantryBackEnd.Services;
using Microsoft.AspNetCore.Http;
namespace PantryBackEnd.Controllers
{
    public class Inventorycontroller : ControllerBase
    {

        private Guid guid = Guid.NewGuid();
        JwtService service;
        private IInventoryRepo InvRepo;
        private IUserRepo userRepo;
        private IProduct productRep;
        public Inventorycontroller(IInventoryRepo context, JwtService service, IUserRepo userRepo, IProduct productRep)
        {
            this.service = service;
            this.InvRepo = context;
            this.userRepo = userRepo;
            this.productRep = productRep;
        }

        [Route("api/SingleProduct")]
        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductDt dt)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                Account user = userRepo.GetByID(userId);
                int count = Services.Services.getCount(user.InventoryLists, dt);
                InventoryList item = new InventoryList
                {
                    ItemId = dt.productID,
                    ExpDate = dt.exp,
                    AccId = userId,
                    Count = dt.count,
                    NotificationTime = dt.NotificationTime
                };
                if (count == 0)
                {
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
                        item.Count += count;
                        InvRepo.updateItem(item);
                    }
                    );
                }

                return Ok();

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
            catch (Exception)
            {
                return Unauthorized();
            }

        }

        [Route("api/QRProducts")]
        [HttpPost]
        public async Task<IActionResult> AddProductFromQR([FromBody] List<ProductDt> products)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                Account user = userRepo.GetByID(userId);
                if (user.InventoryLists == null)
                {
                    await Task.Run(() =>
                    {
                        foreach (ProductDt a in products)
                        {
                            InvRepo.AddProduct(new InventoryList
                            {
                                ItemId = a.productID,
                                ExpDate = a.exp,
                                AccId = userId
                            });
                        }
                    });
                }
                else
                {
                    await Task.Run(() =>
                    {
                        foreach (ProductDt a in products)
                        {
                            if (Services.Services.FindDuplicate(user.InventoryLists, a) == true)
                            {
                                InvRepo.AddProduct(new InventoryList
                                {
                                    ItemId = a.productID,
                                    ExpDate = a.exp,
                                    AccId = userId,

                                });
                            }
                            else
                            {
                                InvRepo.AddProduct(new InventoryList
                                {
                                    ItemId = a.productID,
                                    ExpDate = a.exp,
                                    AccId = userId
                                });
                            }
                        }
                    }
                    );
                }
            }
            catch (Exception)
            {
                return Unauthorized();
            }
            return Ok();
        }
    }
}