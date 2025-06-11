using BookStoreAPI.Data;
using BookStoreAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.PromotionService.Repositories
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly ApplicationDbContext _context;

        public PromotionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Promotion>> GetAllAsync()
        {
            return await _context.Promotions.ToListAsync();
        }

        public async Task<Promotion?> GetByIdAsync(Guid id)
        {
            return await _context.Promotions.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Promotion?> GetByNameAsync(string name)
        {
            return await _context.Promotions.FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());
        }

        public async Task<IEnumerable<Promotion>> SearchByKeywordAsync(string keyword)
        {
            return string.IsNullOrWhiteSpace(keyword)
                ? await _context.Promotions.ToListAsync()
                :await _context.Promotions.Where(p => p.Name.Contains(keyword)).ToListAsync();
        }

        public async Task AddAsync(Promotion promotion)
        {
            await _context.AddAsync(promotion);
        }

        public void Update(Promotion promotion)
        {
            _context.Update(promotion);
        }

        public void Delete(Promotion promotion)
        {
            promotion.IsDeleted = true;
            _context.Update(promotion);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
