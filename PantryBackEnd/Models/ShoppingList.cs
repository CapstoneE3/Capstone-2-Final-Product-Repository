﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PantryBackEnd.Models
{
    [Table("Shopping_List")]
    public partial class ShoppingList
    {
        [Key]
        [Column("item_ID", TypeName = "char")]
        public string ItemId { get; set; }
        [Key]
        [Column("acc_ID")]
        public Guid AccId { get; set; }
        [Column("count")]
        public int Count { get; set; }

        [ForeignKey(nameof(AccId))]
        [InverseProperty(nameof(Account.ShoppingLists))]
        public virtual Account Acc { get; set; }
        [ForeignKey(nameof(ItemId))]
        [InverseProperty(nameof(Product.ShoppingLists))]
        public virtual Product Item { get; set; }
    }
}
