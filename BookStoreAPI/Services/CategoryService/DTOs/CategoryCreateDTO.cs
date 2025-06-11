using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Services.CategoryService.DTOs
{
    public class CategoryCreateDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100)]
        [RegularExpression(@".*\S.*", ErrorMessage = "Name cannot be whitespace")]
        public required string Name { get; set; }
    }
}
