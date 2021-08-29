using System;
using System.Collections.Generic;
namespace PantryBackEnd.Models
{
    public class ShoppingList
    {
        List<Product> shopList;

        public ShoppingList(List<Product> shopList)
        {
            this.shopList = shopList;
        }
    }
}