using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookStoreAPI.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Data.Entities;

[Table("Promotion")]
[Index("Name", Name = "IX_Promotion", IsUnique = true)]
public partial class Promotion : BaseEntity
{
    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("startDate")]
    public DateTime StartDate { get; set; }

    [Column("endDate")]
    public DateTime EndDate { get; set; }

    [Column("condition", TypeName = "decimal(8, 0)")]
    public decimal Condition { get; set; }

    [Column("discountPercent", TypeName = "decimal(3, 2)")]
    public decimal DiscountPercent { get; set; }

    [Column("quantity")]
    public short Quantity { get; set; }

    [InverseProperty("Promotion")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
