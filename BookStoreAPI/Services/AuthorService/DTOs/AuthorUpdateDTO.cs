using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Services.AuthorService.DTOs
{
    public class AuthorUpdateDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100)]
        [RegularExpression(@".*\S.*", ErrorMessage = "Name cannot be whitespace")]
        public required string Name { get; set; }

        public required bool IsDeleted { get; set; }
    }
}
