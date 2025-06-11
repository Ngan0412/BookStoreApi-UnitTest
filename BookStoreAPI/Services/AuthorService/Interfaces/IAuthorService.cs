using BookStoreAPI.Services.AuthorService.DTOs;

namespace BookStoreAPI.Services.AuthorService.Interfaces
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorDTO>> GetAllAsync();

        Task<AuthorDTO?> GetByIdAsync(Guid id);

        Task<IEnumerable<AuthorDTO>> SearchByKeywordAsync(string keyword);

        Task<bool> AddAsync (AuthorCreateDTO authorCreateDTO);

        Task<bool> UpdateAsync(Guid id, AuthorUpdateDTO authorUpdateDTO);

        Task<bool> DeleteAsync(Guid id);
    }
}
