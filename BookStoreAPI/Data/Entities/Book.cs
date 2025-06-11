using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookStoreAPI.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Data.Entities;

[Table("Book")]
[Index("Isbn", Name = "IX_Book", IsUnique = true)]
public partial class Book : BaseEntity
{
    [Column("isbn")]
    [StringLength(13)]
    [Unicode(false)]
    public string Isbn { get; set; } = null!;

    [Column("title")]
    [StringLength(100)]
    public string Title { get; set; } = null!;

    [Column("categoryId")]
    public Guid CategoryId { get; set; }

    [Column("authorId")]
    public Guid AuthorId { get; set; }

    [Column("publisherId")]
    public Guid PublisherId { get; set; }

    [Column("yearOfPublication")]
    public short YearOfPublication { get; set; }

    [Column("price", TypeName = "decimal(8, 0)")]
    public decimal Price { get; set; }

    [Column("image")]
    [StringLength(255)]
    [Unicode(false)]
    public string Image { get; set; } = null!;

    [Column("quantity")]
    public int Quantity { get; set; }

    [ForeignKey("AuthorId")]
    [InverseProperty("Books")]
    public virtual Author Author { get; set; } = null!;

    [ForeignKey("CategoryId")]
    [InverseProperty("Books")]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("Book")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [ForeignKey("PublisherId")]
    [InverseProperty("Books")]
    public virtual Publisher Publisher { get; set; } = null!;
}
