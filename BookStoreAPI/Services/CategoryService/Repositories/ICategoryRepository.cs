using BookStoreAPI.Data.Entities;
using System.Threading.Tasks;

namespace BookStoreAPI.Services.CategoryService.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();

        Task<Category?> GetByIdAsync(Guid Id);

        Task<Category?> GetByNameAsync(string Name);

        Task<IEnumerable<Category>> SearchByKeywordAsync(string keyword);

        Task AddAsync(Category category);

        void Update(Category category);

        void Delete(Category category);

        Task<bool> SaveChangesAsync();
    }
}
