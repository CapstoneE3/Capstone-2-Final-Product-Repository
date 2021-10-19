using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PantryBackEnd.Models
{
    public class frontEndRecipeStep
    {
        public int RecipeId { get; set; }
        public List<string> Instructions { get; set; }
    }
}