using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
namespace PantryBackEnd.Models
{
    public class Subscription
    {
        public Guid acc_ID { get; set; }
        public string Endpoint { get; set; }

        public Dictionary<string, string> Keys { get; set; }
    }
}