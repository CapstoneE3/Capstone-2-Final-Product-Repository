using System;
using System.Collections.Generic;
namespace PantryBackEnd.Models
{
    public class Inventory
    {
        List<Product> inventoryList = new List<Product>();

        public Inventory(List<Product> inventoryList)
        {
            this.inventoryList = inventoryList;
        }
    }
}