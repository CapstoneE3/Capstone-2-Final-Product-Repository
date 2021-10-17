using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PantryBackEnd.Models
{
    public class SubscriptionData
    {
        public string title { get; set; }
        public List<string> body { get; set; }
        public DateTime expiry { get; set; }
    }
}