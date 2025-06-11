using BookStoreAPI.Data;
using BookStoreAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.AuthService.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;

        public AuthRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Staff?> GetByEmailAsync (string email)
        {
            return await _context.Staff.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<Staff?> GetByPhoneAsync(string phone)
        {
            return await _context.Staff.FirstOrDefaultAsync(x => x.Phone == phone);
        }

        public async Task<Staff?> GetByCitizenIdentificationAsync(string citizenIdentification)
        {
            return await _context.Staff.FirstOrDefaultAsync(x => x.CitizenIdentification == citizenIdentification);
        }

        public async Task AddSync (Staff staff)
        {
            await _context.Staff.AddAsync(staff);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Staff?> GetByIdAsync (Guid id)
        {
            return await _context.Staff.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
