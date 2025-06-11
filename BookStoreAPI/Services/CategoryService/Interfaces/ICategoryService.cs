using BookStoreAPI.Services.CategoryService.DTOs;

namespace BookStoreAPI.Services.CategoryService.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAllAsync();

        Task<CategoryDTO?> GetByIdAsync(Guid id);

        Task<CategoryDTO?> GetByNameAsync(string name);

        Task<IEnumerable<CategoryDTO>> SearchByKeywordAsync(string keyword);

        Task<bool> AddAsync(CategoryCreateDTO categoryCreateDTO);

        Task<bool> UpdateAsync(Guid id, CategoryUpdateDTO categoryUpdateDTO);

        Task<bool> DeleteAsync(Guid id);
    }
}
