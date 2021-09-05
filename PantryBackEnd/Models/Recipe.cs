using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PantryBackEnd.Models
{
    public partial class Recipe
    {
        [Key]
        [Column("recipe_ID")]
        public int RecipeId { get; set; }
        [Required]
        [Column("steps", TypeName = "char")]
        public string Steps { get; set; }
        [Required]
        [Column("recipe_name", TypeName = "char")]
        public string RecipeName { get; set; }

        [InverseProperty("Recipe")]
        public virtual RecipeList RecipeList { get; set; }
    }
}
