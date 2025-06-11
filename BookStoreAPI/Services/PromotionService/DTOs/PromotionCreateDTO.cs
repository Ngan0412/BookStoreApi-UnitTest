using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Services.PromotionService.DTOs
{
    public class PromotionCreateDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100)]
        [RegularExpression(@".*\S.*", ErrorMessage = "Name cannot be whitespace")]
        public required string Name { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public required decimal Condition { get; set; }
        public required decimal DiscountPercent { get; set; }
        public required short Quantity { get; set; }
    }
}
