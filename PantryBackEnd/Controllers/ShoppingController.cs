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
        [Route("api/AddShoppingItem")]
        [HttpPost]
        public async Task<ActionResult> AddShoppingItem([FromBody] ShoppingItems items)
        {
            try
            {

                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                ShoppingList shop = new ShoppingList
                {
                    ItemId = items.ItemId,
                    Count = items.Count,
                    AccId = userId
                };
                await shopping.addShoppingItems(shop);
                return Ok();

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
            catch (Exception)
            {
                return Unauthorized();
            }
        }


        public ActionResult<List<Product>> GetProducts(int index)
        {
            try
            {

                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                List<Product> pro = product.fetchProducts(index);
                return Ok(pro);
            }
            catch (Exception)
            {
                return Unauthorized();
            }


        }

        [Route("api/adaddProductFromListdProt")]
        [HttpPost]
        public async Task<ActionResult> addProductsFromList(List<string> prods)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = service.Verification(jwt);
                Guid userId = Guid.Parse(token.Issuer);
                List<ShoppingList> shopList = new List<ShoppingList>();

                foreach(string a in prods)
                {
                    ShoppingList shop = new ShoppingList
                    {
                        ItemId = a,
                        Count = 1,
                        AccId = userId
                    };
                    shopList.Add(shop);
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