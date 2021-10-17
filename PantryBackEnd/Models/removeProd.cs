using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PantryBackEnd.Models
{
    public class removeProd
    {
        public string productID { get; set; }
        public DateTime exp { get; set; }

        public int count { get; set; }
    }
}