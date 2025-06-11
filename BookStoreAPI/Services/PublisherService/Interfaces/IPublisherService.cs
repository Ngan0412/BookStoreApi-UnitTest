using BookStoreAPI.Services.PublisherService.DTOs;

namespace BookStoreAPI.Services.PublisherService.Interfaces
{
    public interface IPublisherService
    {
        Task<IEnumerable<PublisherDTO>> GetAllAsync();

        Task<PublisherDTO?> GetByIdAsync(Guid id);

        Task<PublisherDTO?> GetByNameAsync(string name);

        Task<IEnumerable<PublisherDTO>> SearchByKeywordAsync(string keyword);

        Task<bool> AddAsync(PublisherCreateDTO publisherCreateDTO);

        Task<bool> UpdateAsync(Guid id, PublisherUpdateDTO publisherUpdateDTO);

        Task<bool> DeleteAsync(Guid id);
    }
}
