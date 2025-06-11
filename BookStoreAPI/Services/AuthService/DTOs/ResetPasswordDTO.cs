using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Services.AuthService.DTOs
{
    public class ResetPasswordDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [StringLength(50)]
        [RegularExpression(@"\S+", ErrorMessage = "Email cannot be whitespace")]
        public required String Email { get; set; }
    }
}
