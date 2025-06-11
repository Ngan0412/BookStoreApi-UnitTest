using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookStoreAPI.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Data.Entities;

[Table("Author")]
public partial class Author : BaseEntity
{
    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [InverseProperty("Author")]
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
