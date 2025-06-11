using System.Security.Claims;

namespace BookStoreAPI.Common.Helpers
{
    // Dung static de khong can phai tao instance, tien loi cho viec su dung nhanh o moi noi.
    public static class CurrentUserHelper
    {
        public static Guid GetStaffId(ClaimsPrincipal user)
        {
            var staffIdClaim = user.FindFirst("staffId")?.Value;
            if (string.IsNullOrEmpty(staffIdClaim))
                throw new UnauthorizedAccessException("Staff ID not found in token.");

            return Guid.Parse(staffIdClaim);
        }

        public static bool IsAdmin(ClaimsPrincipal user)
        {
            return user.IsInRole("Admin");
        }
    }
}
