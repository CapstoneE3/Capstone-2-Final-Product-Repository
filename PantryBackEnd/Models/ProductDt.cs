using System;
namespace PantryBackEnd.Models
{
    public class ProductDt
    {
        public string productID { get; set; }
        public DateTime exp { get; set; }

        public int count { get; set; }
        public DateTime NotificationTime { get; set; }
    }
}