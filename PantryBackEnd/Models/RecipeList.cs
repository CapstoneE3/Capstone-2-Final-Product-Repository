using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PantryBackEnd.Models
{
    [Table("Recipe_List")]
    public partial class RecipeList
    {
        [Key]
        [Column("recipe_ID")]
        public int RecipeId { get; set; }
        [Required]
        [Column("item_ID", TypeName = "char")]
        public string ItemId { get; set; }
        [Column("acc_ID")]
        public Guid AccId { get; set; }
        [Required]
        [Column("quantity", TypeName = "char")]
        public string Quantity { get; set; }

        [ForeignKey(nameof(AccId))]
        [InverseProperty(nameof(Account.RecipeLists))]
        public virtual Account Acc { get; set; }
        [ForeignKey(nameof(ItemId))]
        [InverseProperty(nameof(Product.RecipeLists))]
        public virtual Product Item { get; set; }
        [ForeignKey(nameof(RecipeId))]
        [InverseProperty("RecipeList")]
        public virtual Recipe Recipe { get; set; }
    }
}
