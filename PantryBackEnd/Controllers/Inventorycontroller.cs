using Microsoft.AspNetCore.Mvc;
using PantryBackEnd.JwtGenerator;
using PantryBackEnd.Models;
using System;
using PantryBackEnd.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;
using PantryBackEnd.Services;
namespace PantryBackEnd.Controllers
{
    public class Inventorycontroller : ControllerBase
    {
        
        private Guid guid = Guid.NewGuid();
        JwtService service;
        private IInventoryRepo InvRepo;
        private IUserRepo userRepo;
        public Inventorycontroller(IInventoryRepo context, JwtService service, IUserRepo userRepo)
        {
            this.service = service;
            this.InvRepo = context;
            this.userRepo = userRepo;
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
                if(user.InventoryLists == null)
                {
                    InvRepo.AddProduct(new InventoryList{
                        ItemId = dt.productID,
                        ExpDate = dt.exp,
                        AccId = userId
                    });
                    itemsExist =true;
                }
                else if(await Task.Run(()=> Services.Services.FindDuplicate(user.InventoryLists,dt) ==true))
                {
                    InvRepo.AddProduct(new InventoryList{
                    ItemId = dt.productID,
                    ExpDate = dt.exp,
                    AccId = userId,
                    DuplicateId = guid
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

            return Ok();    
            

        }
        [Route("api/GetInventoryList")]
        [HttpGet]
        public ActionResult<ICollection<InventoryList>> GetInventoryLists()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                Account user = userRepo.GetByID(userId);
                return Ok(user.InventoryLists);
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
                                DuplicateId = guid
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
    }
}