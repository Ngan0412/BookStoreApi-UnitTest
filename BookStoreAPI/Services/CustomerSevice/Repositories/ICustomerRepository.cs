using BookStoreAPI.Data.Entities;

namespace BookStoreAPI.Services.CustomerSevice.Repositories
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(Guid id);
        Task<Customer?> GetByPhoneAsync(string phone);
        Task<IEnumerable<Customer>> SearchByKeywordAsync(string keyword);
        Task AddAsync(Customer customer);
        void Update(Customer customer);
        void Delete(Customer customer);
        Task<bool> SaveChangesAsync();
    }
}
