using BookStoreAPI.Data;
using BookStoreAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.PublisherService.Repositories
{
    public class PublisherRepository : IPublisherRepository
    {
        private readonly ApplicationDbContext _context;

        public PublisherRepository (ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Publisher>> GetAllAsync ()
        {
            return await _context.Publishers.ToListAsync();
        }

        public async Task<Publisher?> GetByIdAsync (Guid id)
        {
            return await _context.Publishers.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Publisher?> GetByNameAsync(string name)
        {
            return await _context.Publishers.FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());
        }

        public async Task<IEnumerable<Publisher>> SearchByKeywordAsync (string keyword)
        {
            return string.IsNullOrWhiteSpace(keyword)
                ? await _context.Publishers.ToListAsync()
                : await _context.Publishers.Where(p => p.Name.Contains(keyword)).ToListAsync();
        }

        public async Task AddAsync (Publisher publisher)
        {
            await _context.Publishers.AddAsync(publisher);
        }

        public void Update (Publisher publisher)
        {
            _context.Publishers.Update(publisher);
        }

        public void Delete (Publisher publisher)
        {
            publisher.IsDeleted = true;

            _context.Publishers.Update(publisher);
        }

        public async Task<bool> SaveChangesAsync ()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
