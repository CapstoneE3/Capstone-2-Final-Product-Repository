using System;
namespace PantryBackEnd.Models
{
    public class Product
    {
        private string name;
        private DateTime expiryDate;
        private string id;

        public Product(string name, DateTime expiryDate, string id)
        {
            this.name = name;
            this.expiryDate = expiryDate;
            this.id = id;
        }
    }
}