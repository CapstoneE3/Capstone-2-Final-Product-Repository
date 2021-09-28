using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PantryBackEnd.Models
{
    public class VapidDt
    {
        public string subjecy { get; set; }
        public string publicKey { get; set; }
        public string privateKey { get; set; }

        public VapidDt(string privateKey, string publicKey, string subjecy)
        {
            this.privateKey = privateKey;
            this.publicKey = publicKey;
            this.subjecy = subjecy;
        }
    }
}