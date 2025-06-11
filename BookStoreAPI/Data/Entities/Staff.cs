using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookStoreAPI.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Data.Entities;

[Index("CitizenIdentification", Name = "IX_Staff_CI", IsUnique = true)]
[Index("Email", Name = "IX_Staff_Email", IsUnique = true)]
[Index("Phone", Name = "IX_Staff_Phone", IsUnique = true)]
public partial class Staff : BaseEntity
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

    [Column("email")]
    [StringLength(50)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Column("citizenIdentification")]
    [StringLength(12)]
    [Unicode(false)]
    public string CitizenIdentification { get; set; } = null!;

    [Column("hashPassword")]
    [StringLength(255)]
    [Unicode(false)]
    public string HashPassword { get; set; } = null!;

    [Column("role")]
    public bool Role { get; set; }

    [Column("gender")]
    public bool Gender { get; set; }

    [Column("isActived")]
    public bool IsActived { get; set; }

    [InverseProperty("Staff")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
