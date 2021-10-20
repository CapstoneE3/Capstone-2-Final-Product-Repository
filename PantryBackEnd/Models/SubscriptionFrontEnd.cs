using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
namespace PantryBackEnd.Models
{
    public class SubscriptionFrontEnd
    {
        public string endpoint { get; set; }

        public Dictionary<string, string> keys { get; set; }
    }
}