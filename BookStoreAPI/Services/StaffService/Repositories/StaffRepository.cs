using BookStoreAPI.Data;
using BookStoreAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.StaffService.Repositories
{
    public class StaffRepository : IStaffRepository
    {
        private readonly ApplicationDbContext _context;

        public StaffRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Staff>> GetAllAsync()
        {
            return await _context.Staff.ToListAsync();
        }

        public async Task<Staff?> GetByIdAsync(Guid id)
        {
            return await _context.Staff.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Staff?> GetByPhoneAsync(string phone)
        {
            return await _context.Staff.FirstOrDefaultAsync(s => s.Phone == phone);
        }

        public async Task<Staff?> GetByEmailAsync(string email)
        {
            return await _context.Staff.FirstOrDefaultAsync(s => s.Email.ToLower() == email.ToLower());
        }

        public async Task<Staff?> GetByCitizenIdentificationAsync(string citizenIdentification)
        {
            return await _context.Staff.FirstOrDefaultAsync(s => s.CitizenIdentification == citizenIdentification);
        }
        
        public async Task<IEnumerable<Staff>> SearchByKeyword(string keyword)
        {
            return string.IsNullOrWhiteSpace(keyword)
                ? await _context.Staff.ToListAsync()
                : await _context.Staff
                .Where(p => p.FamilyName.Contains(keyword) || p.GivenName.Contains(keyword) || p.Address.Contains(keyword)
                || p.Phone.Contains(keyword)|| p.Email.Contains(keyword) || p.CitizenIdentification.Contains(keyword))
                .ToListAsync();
        }

        public void Update(Staff staff)
        {
            _context.Staff.Update(staff);
        }

        public void Delete(Staff staff)
        {
            staff.IsDeleted = true;

            _context.Update(staff);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        
    }
}
