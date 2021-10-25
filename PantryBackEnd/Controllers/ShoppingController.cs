using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PantryBackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using PantryBackEnd.JwtGenerator;
using PantryBackEnd.Repositories;

namespace PantryBackEnd.Controllers
{
    public class ShoppingController : ControllerBase
    {
        JwtService service;
        IShoppingList shopping;
        IProduct product;

        public ShoppingController(JwtService service, IShoppingList shoppingList, IProduct product)
        {
            this.service = service;
            this.shopping = shoppingList;
            this.product = product;
        }
        public int ShoppingCount(ICollection<ShoppingList> list, ShoppingItems product)
        {
            foreach (ShoppingList a in list)
            {
                if (product.ItemId.Equals(a.ItemId))
                {
                    a.Count += product.Count;
                    return (int)a.Count;
                }
            }
            return 0;
        }
        [Route("api/AddShoppingItem")]
        [HttpPost]
        public async Task<ActionResult> AddShoppingItem([FromBody] ShoppingItems items)
        {

            try
            {
                // check if the request have a valid cookie
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                Account user = shopping.getUserShoppingList(userId);
                int count = ShoppingCount(user.ShoppingLists, items);
                if (count == 0)
                {
                    ShoppingList shop = new ShoppingList
                    {
                        ItemId = items.ItemId,
                        Count = items.Count,
                        AccId = userId
                    };
                    await Task.Run(() =>
                    {
                        shopping.addShoppingItems(shop);
                    }
                    );
                }
                else
                {
                    shopping.UpdateShop();
                }

                return Ok();

            }
            catch (Exception e)
            {
                return Unauthorized();
            }
        }

        [Route("api/AddShoppingItemList")]
        [HttpPost]
        public async Task<ActionResult> AddShoppingItemList([FromBody] List<ShoppingItems> items)
        {
            try
            {

                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                Account user = shopping.getUserShoppingList(userId);
                List<ShoppingList> list = new List<ShoppingList>();
                if (user.ShoppingLists == null || user.ShoppingLists.Count == 0)
                {
                    await Task.Run(() =>
                    {
                        foreach (ShoppingItems a in items)
                        {
                            ShoppingList inv = new ShoppingList { ItemId = a.ItemId, Count = a.Count, AccId = userId };
                            list.Add(inv);
                        }
                    });
                    await shopping.AddShoppingList(list);
                }
                else
                {
                    await Task.Run(() =>
                   {
                       int count = 0;
                       foreach (ShoppingItems a in items)
                       {
                           count = ShoppingCount(user.ShoppingLists, a);
                           if (count == 0)
                           {
                               ShoppingList inv = new ShoppingList { ItemId = a.ItemId, Count = a.Count, AccId = userId };
                               shopping.addShoppingItems(inv);
                           }
                           else
                           {
                               shopping.UpdateShop();
                           }
                       }
                   });
                }
                return Ok(new { message = "Success" });

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

        [Route("api/DeleteShoppingItem")]
        [HttpDelete]
        public async Task<ActionResult> DeleteShoppingItem([FromBody] ShoppingItems items)
        {
            try
            {

                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                await shopping.DeleteShoppingItem(items.ItemId, userId);
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
        [Route("api/GetShoppingItems")]
        [HttpGet]
        public ActionResult<List<ShoppingItemsFormat>> GetShoppingItems()
        {
            try
            {

                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                return Ok(shopping.getShoppingList(userId));
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

        [Route("api/getShoppingProducts")]
        [HttpGet]
        public ActionResult<List<Product>> GetProducts(int index)
        {
            try
            {

                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                if (index >= 0)
                {
                    List<Product> pro = product.fetchProducts(index);
                    return Ok(pro);

                }
                else
                {
                    List<Product> pro = new List<Product>();
                    return Ok(pro);
                }

            }
            catch (Exception)
            {
                return Unauthorized();
            }


        }

        [Route("api/addProductFromList")]
        [HttpPost]
        public async Task<ActionResult> addProductsFromList([FromBody] List<string> prods)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                List<ShoppingList> shopList = new List<ShoppingList>();

                foreach (string a in prods)
                {
                    ShoppingList shop = new ShoppingList
                    {
                        ItemId = a,
                        Count = 1,
                        AccId = userId
                    };
                    int count = shopping.GetItemCount(shop);
                    if (count > 0)
                    {
                        shop.Count += count;
                        await shopping.updateItems(shop);
                    }
                    else
                    {
                        shopList.Add(shop);
                    }
                }

                await shopping.AddShoppingList(shopList);
                return Ok();
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }
    }
}