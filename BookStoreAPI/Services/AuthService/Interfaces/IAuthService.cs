using BookStoreAPI.Services.AuthService.DTOs;

namespace BookStoreAPI.Services.AuthService.Interfaces
{
    public interface IAuthService
    {
        // Dang ky tk moi cho Staff
        Task<bool>RegisterAsync(RegisterDTO registerDTO);

        // Login -> Tra ve token neu thanh cong
        Task<String?> LoginAsync(LoginDTO loginDTO);

        // Doi mat khau sau khi da Login thanh cong
        Task<bool> ChangePasswordAsync(Guid staffID, ChangePasswordDTO changePasswordDTO);

        // Reset mat khau (Gui mail link va reset mk ve "123456"
        Task<bool> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO);
        Task<bool> ResetPasswordFromTokenAsync(string token);

        // Kich hoa tai khoan (click xac nhan ben mail)
        Task<bool> ActivateAccountAsync(String token);
    }
}
