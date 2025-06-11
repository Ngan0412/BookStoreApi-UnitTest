using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookStoreAPI.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Data.Entities;

[Table("Customer")]
[Index("Phone", Name = "IX_Customer", IsUnique = true)]
public partial class Customer : BaseEntity
{
    [Column("familyName")]
    [StringLength(70)]
    public string FamilyName { get; set; } = null!;

    [Column("givenName")]
    [StringLength(30)]
    public string GivenName { get; set; } = null!;

    [Column("dateOfBirth")]
    public DateOnly DateOfBirth { get; set; }

    [Column("address")]
    [StringLength(50)]
    public string Address { get; set; } = null!;

    [Column("phone")]
    [StringLength(10)]
    [Unicode(false)]
    public string Phone { get; set; } = null!;

    [Column("gender")]
    public bool Gender { get; set; }

    [InverseProperty("Customer")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
