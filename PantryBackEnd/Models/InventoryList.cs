using System;
using System.Collections.Generic;

#nullable disable

namespace PantryBackEnd.Models
{
    public partial class InventoryList
    {
        public string ItemId { get; set; }
        public Guid AccId { get; set; }
        public DateTime ExpDate { get; set; }
        public int? Count { get; set; }
        public DateTime? NotificationTime { get; set; }

        public virtual Account Acc { get; set; }
        public virtual Product Item { get; set; }
    }
}
