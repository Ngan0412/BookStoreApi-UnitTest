using BookStoreAPI.Services.StaffService.DTOs;

namespace BookStoreAPI.Services.StaffService.Interfaces
{
    public interface IStaffService
    {
        Task<IEnumerable<StaffDTO>> GetAllAsync();
        Task<StaffDTO?> GetByIdAsync(Guid id);
        Task<StaffDTO?> GetByPhoneAsync(string phone);
        Task<StaffDTO?> GetByEmailAsync(string email);
        Task<StaffDTO?> GetByCitizenIdentificationAsync(string citizenIdentification);
        Task<IEnumerable<StaffDTO>> SearchByKeywordAsync(string keyword);
        Task<bool> UpdateAsync(Guid id, StaffUpdateDTO staffUpdateDTO);
        Task<bool> DeleteAsync(Guid id);
    }
}
