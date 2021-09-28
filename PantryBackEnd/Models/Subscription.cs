using System;
using System.Collections.Generic;

#nullable disable

namespace PantryBackEnd.Models
{
    public partial class Subscription
    {
        public Guid AccId { get; set; }
        public string SubEndpoint { get; set; }
        public string Key { get; set; }
        public string Audh { get; set; }
        public DateTime ExpNotif { get; set; }

        public virtual Account Acc { get; set; }
    }
}
