using BookStoreAPI.Data;
using BookStoreAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace BookStoreAPI.Services.BookService.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books
                .Include(b => b.Category)
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(Guid id)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book?> GetByIsbnAsync(string isbn)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.Isbn == isbn);
        }

        public async Task<IEnumerable<Book>> SearchByKeywordAsync(string keyword)
        {
            return string.IsNullOrWhiteSpace(keyword)
                ? await _context.Books
                .Include(b => b.Category)
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .ToListAsync()

                : await _context.Books
                .Where(b => b.Title.Contains(keyword) || b.Category.Name.Contains(keyword) || b.Author.Name.Contains(keyword) || b.Publisher.Name.Contains(keyword))
                .Include(b => b.Category)
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .ToListAsync();
        }

        public async Task AddAsync(Book book)
        {
            await _context.AddAsync(book);
        }

        public void Update(Book book)
        {
            _context.Update(book);
        }

        public void Delete(Book book)
        {
            book.IsDeleted = true;

            _context.Update(book);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
