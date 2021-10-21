using System;
using System.Collections.Generic;

#nullable disable

namespace PantryBackEnd.Models
{
    public partial class RecipeDocument
    {
        public int RecipeId { get; set; }
        public string Url { get; set; }
        public string PhotoUrl { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}
