using BookStoreAPI.Data;
using BookStoreAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.CategoryService.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository (ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync (Guid id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category?> GetByNameAsync (string name)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
        }

        public async Task<IEnumerable<Category>> SearchByKeywordAsync(string keyword)
        {
            return string.IsNullOrWhiteSpace(keyword)
                ? await _context.Categories.ToListAsync()
                : await _context.Categories.Where(c => c.Name.Contains(keyword)).ToListAsync();
        }

        public async Task AddAsync (Category category)
        {
            await _context.Categories.AddAsync(category);
        }

        public void Update (Category category)
        {
            _context.Categories.Update(category);
        }

        public void Delete (Category category)
        {
            category.IsDeleted = true;
            _context.Categories.Update(category);
        }

        public async Task<bool> SaveChangesAsync ()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
