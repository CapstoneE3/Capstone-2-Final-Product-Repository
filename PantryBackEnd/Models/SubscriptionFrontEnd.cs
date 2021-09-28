using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
namespace PantryBackEnd.Models
{
    public class SubscriptionFrontEnd
    {
        public string Endpoint { get; set; }

        public string key { get; set; }
        public string auth { get; set; }

        public DateTime expiry { get; set; }
    }
}