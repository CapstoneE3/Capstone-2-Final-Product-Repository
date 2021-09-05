﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PantryBackEnd.Models
{
    public partial class Product
    {
        public Product()
        {
            InventoryLists = new HashSet<InventoryList>();
            RecipeLists = new HashSet<RecipeList>();
            ShoppingLists = new HashSet<ShoppingList>();
        }

        [Key]
        [Column("item_ID", TypeName = "char")]
        public string ItemId { get; set; }
        [Required]
        [Column("quantity", TypeName = "char")]
        public string Quantity { get; set; }
        [Required]
        [Column("category", TypeName = "char")]
        public string Category { get; set; }
        [Required]
        [Column("price", TypeName = "char")]
        public string Price { get; set; }
        [Required]
        [Column("name", TypeName = "char")]
        public string Name { get; set; }

        [InverseProperty(nameof(InventoryList.Item))]
        public virtual ICollection<InventoryList> InventoryLists { get; set; }
        [InverseProperty(nameof(RecipeList.Item))]
        public virtual ICollection<RecipeList> RecipeLists { get; set; }
        [InverseProperty(nameof(ShoppingList.Item))]
        public virtual ICollection<ShoppingList> ShoppingLists { get; set; }
    }
}
