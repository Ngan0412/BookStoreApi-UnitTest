using BookStoreAPI.Data;
using BookStoreAPI.Data.Entities;
using BookStoreAPI.Services.AuthorService.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.AuthorService.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _context;

        public AuthorRepository (ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _context.Authors.ToListAsync();
        }

        public async Task<Author?> GetByIdAsync (Guid id)
        {
            return await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Author>> SearchByKeywordAsync(string keyword)
        {
            return string.IsNullOrWhiteSpace(keyword)
                ? await _context.Authors.ToListAsync()
                : await _context.Authors.Where(a => a.Name.Contains(keyword)).ToListAsync();
        }

        public async Task AddAsync (Author author)
        {
            await _context.Authors.AddAsync(author);
        }

        public void Update (Author author)
        {
            _context.Authors.Update(author);
        }

        public void Delete (Author author)
        {
            author.IsDeleted = true;
            _context.Authors.Update(author);
        }

        public async Task<bool> SaveChangesAsync ()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
