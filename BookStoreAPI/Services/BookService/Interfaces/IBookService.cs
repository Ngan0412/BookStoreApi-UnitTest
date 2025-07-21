using BookStoreAPI.Services.BookService.DTOs;

namespace BookStoreAPI.Services.BookService.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookDetailDTO>> GetAllAsync();
        Task<IEnumerable<BookDetailDTO>> GetPagedAsync(int pageNumber, int pageSize);

        Task<BookDTO?> GetByIdAsync(Guid id);
        Task<BookDTO?> GetByIsbnAsync(string isbn);
        Task<IEnumerable<BookDetailDTO>> SearchByKeywordAsync(string keyword);
        Task<bool> AddAsync(BookCreateDTO bookCreateDTO);
        Task<bool> UpdateAsync(Guid id, BookUpdateDTO bookUpdateDTO);
        Task<bool> DeleteAsync(Guid id);
    }
}
