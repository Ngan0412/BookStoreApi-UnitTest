using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Services.BookService.DTOs
{
    public class BookUpdateDTO
    {
        [Required(ErrorMessage = "ISSBN is required.")]
        [StringLength(13)]
        [RegularExpression(@"\S+", ErrorMessage = "Name cannot be whitespace")]
        public required string Isbn { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100)]
        [RegularExpression(@".*\S.*", ErrorMessage = "Name cannot be whitespace")]
        public required string Title { get; set; }
        public required Guid CategoryId { get; set; }
        public required Guid AuthorId { get; set; }
        public required Guid PublisherId { get; set; }
        public required short YearOfPublication { get; set; }
        public required decimal Price { get; set; }
        public required string Image { get; set; }
        public required int Quantity { get; set; }
        public required bool IsDeleted { get; set; }
    }
}
