using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Services.CustomerSevice.DTOs
{
    public class CustomerUpdateDTO
    {
        [Required(ErrorMessage = "Family name is required.")]
        [StringLength(70)]
        [RegularExpression(@".*\S.*", ErrorMessage = "Family name cannot be whitespace")]
        public required string FamilyName { get; set; }

        [Required(ErrorMessage = "Given name is required.")]
        [StringLength(30)]
        [RegularExpression(@".*\S.*", ErrorMessage = "Given name cannot be whitespace")]
        public required string GivenName { get; set; }
        public required DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(50)]
        [RegularExpression(@".*\S.*", ErrorMessage = "Address cannot be whitespace")]
        public required string Address { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        [StringLength(10)]
        [RegularExpression(@"\S+", ErrorMessage = "Phone cannot be whitespace")]
        public required string Phone { get; set; }
        public required bool Gender { get; set; }
        public required bool IsDeleted { get; set; }
    }
}
