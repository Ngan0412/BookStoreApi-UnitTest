using BookStoreAPI.Data.Entities;
using BookStoreAPI.Services.AuthorService.DTOs;

namespace BookStoreAPI.Services.AuthorService.Repositories
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAllAsync();

        Task<Author?> GetByIdAsync(Guid id);

        Task<IEnumerable<Author>> SearchByKeywordAsync(string keyword);

        Task AddAsync(Author author);

        void Update(Author author);

        void Delete(Author author);

        Task<bool> SaveChangesAsync();
    }
}
