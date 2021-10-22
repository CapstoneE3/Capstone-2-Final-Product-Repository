using System;
using System.Collections.Generic;

#nullable disable

namespace PantryBackEnd.Models
{
    public partial class RecipeList
    {
        public int RecipeId { get; set; }
        public Guid AccId { get; set; }

        public virtual Account Acc { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}
