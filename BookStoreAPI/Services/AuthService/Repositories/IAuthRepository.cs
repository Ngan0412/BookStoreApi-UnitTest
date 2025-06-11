using BookStoreAPI.Data.Entities;

namespace BookStoreAPI.Services.AuthService.Repositories
{
    public interface IAuthRepository
    {
        Task<Staff?> GetByEmailAsync(string email);
        Task<Staff?> GetByPhoneAsync(string phone);
        Task<Staff?> GetByCitizenIdentificationAsync(string citizenIdentification);
        Task<Staff?> GetByIdAsync(Guid id);
        Task AddSync(Staff staff);
        Task<bool> SaveChangesAsync();
    }
}
