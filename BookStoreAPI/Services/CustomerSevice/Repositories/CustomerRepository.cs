using BookStoreAPI.Data;
using BookStoreAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.CustomerSevice.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository (ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Customer?> GetByPhoneAsync(string phone)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Phone == phone);
        }

        public async Task<IEnumerable<Customer>> SearchByKeywordAsync(string keyword)
        {
            return string.IsNullOrWhiteSpace(keyword)
                ? await _context.Customers.ToListAsync()
                : await _context.Customers
                .Where(c => c.FamilyName.Contains(keyword) || c.GivenName.Contains(keyword)
                || c.Phone.Contains(keyword) || c.Address.Contains(keyword))
                .ToListAsync();
        }

        public async Task AddAsync(Customer customer)
        {
            await _context.AddAsync(customer);
        }

        public void Update(Customer customer)
        {
            _context.Update(customer);
        }

        public void Delete(Customer customer)
        {
            customer.IsDeleted = true;

            _context.Update(customer);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
