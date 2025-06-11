using BookStoreAPI.Services.PromotionService.DTOs;

namespace BookStoreAPI.Services.PromotionService.Interfaces
{
    public interface IPromotionService
    {
        Task<IEnumerable<PromotionDTO>> GetAllAsync();
        Task<PromotionDTO?> GetByIdAsync(Guid id);
        Task<PromotionDTO?> GetByNameAsync(string name);
        Task<IEnumerable<PromotionDTO>> SearchByKeywordAsync(string keyword);
        Task<bool> AddAsync(PromotionCreateDTO promotionCreateDTO);
        Task<bool> UpdateAsync(Guid id, PromotionUpdateDTO promotionUpdateDTO);
        Task<bool> DeleteAsync(Guid id);
    }
}
