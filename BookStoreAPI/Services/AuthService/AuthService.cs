using BCrypt.Net;
using BookStoreAPI.Data.Entities;
using BookStoreAPI.Services.AuthService.DTOs;
using BookStoreAPI.Services.AuthService.Interfaces;
using BookStoreAPI.Services.AuthService.Repositories;
using BookStoreAPI.Services.EmailService;
using BookStoreAPI.Services.StaffService.DTOs;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStoreAPI.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _config;
        private readonly EmailSenderService _emailService;

        public AuthService(IAuthRepository authRepository, IConfiguration config, EmailSenderService emailService)
        {
            _authRepository = authRepository;
            _config = config;
            _emailService = emailService;
        }

        public async Task<bool> RegisterAsync(RegisterDTO registerDTO)
        {
            var existingByEmail = await _authRepository.GetByEmailAsync(registerDTO.Email);
            var existingByPhone = await _authRepository.GetByPhoneAsync(registerDTO.Phone);
            var existingByCitizenIdentification = await _authRepository.GetByCitizenIdentificationAsync(registerDTO.CitizenIdentification);

            if (existingByEmail != null)
                throw new InvalidOperationException("Email is already in use.");

            if (existingByPhone != null)
                throw new InvalidOperationException("Phone is already in use.");

            if (existingByCitizenIdentification != null)
                throw new InvalidOperationException("Citizen identification is already in use.");

            if (registerDTO.Password != registerDTO.ConfirmPassword)
                throw new ArgumentException("Confirm password does not match password.");

            if (!IsOver18(registerDTO.DateOfBirth))
                throw new ArgumentException("You must be at least 18 years old.");

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password);

            var staff = new Staff
            {
                FamilyName = registerDTO.FamilyName,
                GivenName = registerDTO.GivenName,
                DateOfBirth = registerDTO.DateOfBirth,
                Address = registerDTO.Address,
                Phone = registerDTO.Phone,
                Email = registerDTO.Email,
                CitizenIdentification = registerDTO.CitizenIdentification,
                HashPassword = hashedPassword,
                Gender = registerDTO.Gender,
                Role = false,
                IsActived = false
            };

            await _authRepository.AddSync(staff);

            var saved = await _authRepository.SaveChangesAsync();

            if (saved == false)
                return false;

            var token = GenerateActivationToken(staff.Id);
            var activationLink = $"http://localhost:5208/api/auth/activate?token={token}";

            await _emailService.SendEmailAsync(staff.Email, "Activate your account",
                $"Click on the following link to activate your account.: <a href='{activationLink}'>Verify account</a>");

            Console.WriteLine($"Send email confirm to: {staff.Email} successfully.");

            return true;
        }

        public async Task<bool> ActivateAccountAsync(string token)
        {
            var jwtKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? _config["Jwt:Key"];

            var handler = new JwtSecurityTokenHandler();

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true
            };

            var principal = handler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
            var staffIdClaim = principal.FindFirst("staffId");
            if (staffIdClaim == null) return false;

            var staffId = Guid.Parse(staffIdClaim.Value);
            var staff = await _authRepository.GetByIdAsync(staffId);
            if (staff == null) return false;

            if (staff.IsActived) return false;

            staff.IsActived = true;
            return await _authRepository.SaveChangesAsync();
        }


        private string GenerateActivationToken(Guid staffId)
        {
            var jwtKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
                      ?? _config["Jwt:Key"]
                      ?? throw new Exception("JWT key is missing");

            var jwtIssuer = _config["Jwt:Issuer"];
            var jwtAudience = _config["Jwt:Audience"];
            var expireMinutes = int.Parse(_config["Jwt:ExpireMinutes"] ?? "60");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] { new Claim("staffId", staffId.ToString()) };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<String?> LoginAsync(LoginDTO loginDTO)
        {
            var staff = await _authRepository.GetByEmailAsync(loginDTO.Email);

            if (staff == null)
                throw new UnauthorizedAccessException("Invalid account, please check your email or create new account.");

            if (staff.IsDeleted)
                throw new UnauthorizedAccessException("Your account has been deleted. Please contact the administrator.");

            if (!staff.IsActived)
                throw new UnauthorizedAccessException("Your account has not been activated. Please check your email to active.");

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(loginDTO.Password, staff.HashPassword);

            if (!isValidPassword)
                throw new UnauthorizedAccessException("Invalid password. Please try again.");

            var token = GenerateJwtToken(staff);

            return token;
        }

        private string GenerateJwtToken(Staff user)
        {
            var jwtKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? _config["Jwt:Key"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("staffId", user.Id.ToString()),
                new Claim("email", user.Email),
                new Claim(ClaimTypes.Role, user.Role ? "Admin" : "Staff")
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:ExpireMinutes"] ?? "60")),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
        {
            var staff = await _authRepository.GetByEmailAsync(resetPasswordDTO.Email);

            if (staff == null)
                throw new UnauthorizedAccessException("Invalid account, please check your email or create new account.");

            if (staff.IsDeleted == true)
                throw new UnauthorizedAccessException("Your account has been deleted. Please contact the administrator.");

            if (staff.IsActived == false)
                throw new UnauthorizedAccessException("Your account has not been activated. Please check your email to active.");

            var token = GenerateResetPasswordToken(staff.Id);
            var resetLink = $"http://localhost:5208/api/auth/reset-password?token={token}";

            await _emailService.SendEmailAsync(staff.Email, "Reset Password",
                $"Click the following link to reset your password to default.: <a href='{resetLink}'>Reset Password</a>");

            return true;
        }

        public async Task<bool> ResetPasswordFromTokenAsync(string token)
        {
            var jwtKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? _config["Jwt:Key"];
            var handler = new JwtSecurityTokenHandler();

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true
            };

            try
            {
                var principal = handler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                var staffIdClaim = principal.FindFirst("staffId");
                if (staffIdClaim == null) return false;

                var staffId = Guid.Parse(staffIdClaim.Value);
                var staff = await _authRepository.GetByIdAsync(staffId);
                if (staff == null || staff.IsDeleted) return false;

                staff.HashPassword = BCrypt.Net.BCrypt.HashPassword("123456");
                return await _authRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        private string GenerateResetPasswordToken(Guid staffId)
        {
            var jwtKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? _config["Jwt:Key"];
            var expireMinutes = 15;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("staffId", staffId.ToString()),
                new Claim("purpose", "reset_password")
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> ChangePasswordAsync(Guid staffID, ChangePasswordDTO changePasswordDTO)
        {
            var staff = await _authRepository.GetByIdAsync(staffID);

            if (staff == null)
                throw new UnauthorizedAccessException("Invalid account, please check your email or create new account.");

            if (staff.IsDeleted == true)
                throw new UnauthorizedAccessException("Your account has been deleted. Please contact the administrator.");

            if (staff.IsActived == false)
                throw new UnauthorizedAccessException("Your account has not been activated. Please check your email to active.");

            if (!BCrypt.Net.BCrypt.Verify(changePasswordDTO.OldPassword, staff.HashPassword))
                throw new UnauthorizedAccessException("Invalid old password. Please try again.");

            if (changePasswordDTO.NewPassword != changePasswordDTO.ConfirmNewPassword)
                throw new UnauthorizedAccessException("New password and confirm password do not match.");

            staff.HashPassword = BCrypt.Net.BCrypt.HashPassword(changePasswordDTO.NewPassword);

            return await _authRepository.SaveChangesAsync();
        }

        private bool IsOver18(DateOnly dateOfBirth)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            if (dateOfBirth > today)
                throw new ArgumentException("Date of birth cannot be in the future.");

            int age = today.Year - dateOfBirth.Year;

            if (dateOfBirth > today.AddYears(-age))
                age--;

            return age >= 18;
        }

        // Check strong password
        //private bool IsStrongPassword(string password)
        //{
        //    if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
        //        return false;

        //    bool hasUpper = password.Any(char.IsUpper);
        //    bool hasLower = password.Any(char.IsLower);
        //    bool hasDigit = password.Any(char.IsDigit);
        //    bool hasSpecial = password.Any(ch => "!@#$%^&*()-_=+[]{}|;:'\",.<>?/`~".Contains(ch));

        //    return hasUpper && hasLower && hasDigit && hasSpecial;
        //}

    }
}
