using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PantryBackEnd.Models
{
    [Table("Inventory_List")]
    public partial class InventoryList
    {
        [Key]
        [Column("item_ID", TypeName = "char")]
        public string ItemId { get; set; }
        [Key]
        [Column("acc_ID")]
        public Guid AccId { get; set; }
        [Key]
        [Column("duplicate_ID")]
        public Guid DuplicateId { get; set; }
        [Column("exp_date", TypeName = "date")]
        public DateTime ExpDate { get; set; }

        [ForeignKey(nameof(AccId))]
        [InverseProperty(nameof(Account.InventoryLists))]
        public virtual Account Acc { get; set; }
        [ForeignKey(nameof(ItemId))]
        [InverseProperty(nameof(Product.InventoryLists))]
        public virtual Product Item { get; set; }
    }
}
