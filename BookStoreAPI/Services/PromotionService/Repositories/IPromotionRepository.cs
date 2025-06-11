using BookStoreAPI.Data.Entities;

namespace BookStoreAPI.Services.PromotionService.Repositories
{
    public interface IPromotionRepository
    {
        Task<IEnumerable<Promotion>> GetAllAsync();
        Task<Promotion?> GetByIdAsync(Guid id);
        Task<Promotion?> GetByNameAsync(string name);
        Task<IEnumerable<Promotion>> SearchByKeywordAsync(string keyword);
        Task AddAsync(Promotion promotion);
        void Update(Promotion promotion);
        void Delete(Promotion promotion);
        Task<bool> SaveChangesAsync();
    }
}
