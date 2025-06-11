using BookStoreAPI.Data.Entities;

namespace BookStoreAPI.Services.PublisherService.Repositories
{
    public interface IPublisherRepository
    {
        Task<IEnumerable<Publisher>> GetAllAsync();

        Task<Publisher?> GetByIdAsync(Guid id);

        Task<Publisher?> GetByNameAsync(string name);

        Task<IEnumerable<Publisher>> SearchByKeywordAsync(string keyword);

        Task AddAsync(Publisher publisher);

        void Update(Publisher publisher);

        void Delete (Publisher publisher);

        Task<bool> SaveChangesAsync();
    }
}
