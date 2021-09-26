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
                bool itemsExist = false;
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                Account user = userRepo.GetByID(userId);
                if(user.InventoryLists.Count == 0)
                {
                    InventoryList item = new InventoryList{
                        ItemId = dt.productID,
                        ExpDate = dt.exp,
                        AccId = userId,
                        //Acc = user,
                        //Item = productRep.getProductById(dt.productID)
                        };
                    InvRepo.AddProduct(item);
                    itemsExist =true;                   
                }
                else if(await Task.Run(()=> Services.Services.FindDuplicate(user.InventoryLists,dt) ==true))
                {
                    InvRepo.AddProduct(new InventoryList{
                    ItemId = dt.productID,
                    ExpDate = dt.exp,
                    AccId = userId,
                    
                    });
                    itemsExist =true;   
                }
                if(itemsExist == false)
                {
                    InvRepo.AddProduct(new InventoryList{
                        ItemId = dt.productID,
                        ExpDate = dt.exp,
                        AccId = userId
                    });
                }

            }catch(Exception)
            {
                return Unauthorized();
            }

            return Ok(new {message = "Success"});    
            

        }
        [Route("api/GetInventoryList")]
        [HttpGet]
        public ActionResult<Dictionary <string, object>> GetInventoryLists()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                Account user = userRepo.GetByID(userId);
                var list = InvRepo.GetInventoryList(userId);
                return Ok(list);
            }catch(Exception)
            {
                return Unauthorized();
            }

        }
    
        [Route("api/QRProducts")]
        [HttpPost]
        public async Task<IActionResult> AddProductFromQR(List<ProductDt> products)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                Account user = userRepo.GetByID(userId);
                if(user.InventoryLists == null)
                {
                    await Task.Run(()=>
                    {
                        foreach(ProductDt a in products)
                        {
                            InvRepo.AddProduct(new InventoryList{
                                ItemId = a.productID,
                                ExpDate = a.exp,
                                AccId = userId
                            });
                        }
                    });    
                }
                else
                { 
                    await Task.Run(()=> 
                    {
                        foreach(ProductDt a in products)
                        {
                            if(Services.Services.FindDuplicate(user.InventoryLists,a) ==true)
                            {
                                InvRepo.AddProduct(new InventoryList{
                                ItemId = a.productID,
                                ExpDate = a.exp,
                                AccId = userId,
                                
                                });
                            }
                            else
                            {
                                InvRepo.AddProduct(new InventoryList{
                                ItemId = a.productID,
                                ExpDate = a.exp,
                                AccId = userId
                                });
                            }
                        }
                    }
                    );
                }
            }catch(Exception)
            {
                return Unauthorized();
            }
            return Ok();    
        }

        [Route("api/removeInventoryItem")]
        [HttpDelete]
        public ActionResult removeInventoryItem(ProductDt dt)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                Account user = userRepo.GetByID(userId);

                InvRepo.removeProduct(userId, dt.productID);
                
                return Ok(new {message = "Success"});

            }catch(Exception)
            {
                return Unauthorized(new {message = "Failed"});
            }

        }
    }
}