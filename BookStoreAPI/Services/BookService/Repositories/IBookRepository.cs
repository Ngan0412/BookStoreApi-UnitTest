using BookStoreAPI.Data.Entities;

namespace BookStoreAPI.Services.BookService.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book?> GetByIdAsync(Guid id);
        Task<Book?> GetByIsbnAsync(string isbn);
        Task<IEnumerable<Book>> SearchByKeywordAsync(string keyword);
        Task AddAsync(Book book);
        void Update(Book book);
        void Delete(Book book);
        Task<bool> SaveChangesAsync();
    }
}
