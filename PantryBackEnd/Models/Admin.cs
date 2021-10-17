using System;
using System.Collections.Generic;

#nullable disable

namespace PantryBackEnd.Models
{
    public partial class Admin
    {
        public Guid AccId { get; set; }
        public int Level { get; set; }

        public virtual Account Acc { get; set; }
    }
}
