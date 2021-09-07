using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PantryBackEnd.Models
{
    [Table("Account")]
    public partial class Account
    {
        public Account()
        {
            InventoryLists = new HashSet<InventoryList>();
            RecipeLists = new HashSet<RecipeList>();
            ShoppingLists = new HashSet<ShoppingList>();
        }

        [Key]
        [Column("acc_ID")]
        public Guid AccId { get; set; }
        [Required]
        [Column("name", TypeName = "char")]
        public string Name { get; set; }
        [Required]
        [Column("email", TypeName = "char")]
        public string Email { get; set; }
        [Required]
        [Column("password")]
        [StringLength(64)]
        public string Password { get; set; }

        [InverseProperty(nameof(InventoryList.Acc))]
        public virtual ICollection<InventoryList> InventoryLists { get; set; }
        [InverseProperty(nameof(RecipeList.Acc))]
        public virtual ICollection<RecipeList> RecipeLists { get; set; }
        [InverseProperty(nameof(ShoppingList.Acc))]
        public virtual ICollection<ShoppingList> ShoppingLists { get; set; }
    }
}
