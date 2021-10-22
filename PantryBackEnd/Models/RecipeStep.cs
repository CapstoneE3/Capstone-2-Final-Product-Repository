using System;
using System.Collections.Generic;

#nullable disable

namespace PantryBackEnd.Models
{
    public partial class RecipeStep
    {
        public int RecipeId { get; set; }
        public int StepId { get; set; }
        public string Instructions { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
