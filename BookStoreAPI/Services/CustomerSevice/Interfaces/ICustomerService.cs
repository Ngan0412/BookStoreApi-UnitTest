using BookStoreAPI.Services.CustomerSevice.DTOs;

namespace BookStoreAPI.Services.CustomerSevice.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDTO>> GetAllAsync();
        Task<CustomerDTO?> GetByIdAsync(Guid id);
        Task<CustomerDTO?> GetByPhoneAsync(string phone);
        Task<IEnumerable<CustomerDTO>> SearchByKeywordAsync(string keyword);
        Task<bool> AddAsync(CustomerCreateDTO customerCreateDTO);
        Task<bool> UpdateAsync(Guid id, CustomerUpdateDTO customerUpdateDTO);
        Task<bool> DeleteAsync(Guid id);
    }
}
