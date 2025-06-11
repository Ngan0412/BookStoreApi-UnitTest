using BookStoreAPI.Common.Controllers;
using BookStoreAPI.Services.AuthService.DTOs;
using BookStoreAPI.Services.AuthService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers
{
    [AllowAnonymous]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            var success = await _authService.RegisterAsync(registerDTO);

            if (!success)
                return BadRequest("Registration failed. Please check your information.");

            return Ok("Registration successful. Please check your email to activate your account.");
        }

        [HttpGet("activate")]
        public async Task<IActionResult> ActivateAccount([FromQuery] string token)
        {
            var success = await _authService.ActivateAccountAsync(token);

            if (!success)
                return BadRequest("Activation failed. Please check your token.");

            return Ok("Account activated successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                var token = await _authService.LoginAsync(loginDTO);

                return Ok(new { token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDTO)
        {
            try
            {
                await _authService.ResetPasswordAsync(resetPasswordDTO);
                return Ok(new { message = "Reset password email sent successfully." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPassword([FromQuery] string token)
        {
            var success = await _authService.ResetPasswordFromTokenAsync(token);

            if (!success)
                return BadRequest( new { message = "Token is invalid or expried." });

            return Ok(new { message = "Password reset successfully. Your new password is: '123456'" });
        }

        [HttpPut("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            try
            {
                var staffIdClaim = User.FindFirst("staffId");

                if (staffIdClaim == null)
                    return Unauthorized(new { message = "Invalid token." });

                var staffId = Guid.Parse(staffIdClaim.Value);

                var success = await _authService.ChangePasswordAsync(staffId, changePasswordDTO);

                if (!success)
                    return BadRequest(new { message = "Change password failed. Please check your information." });

                return Ok(new { message = "Password changed successfully." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
