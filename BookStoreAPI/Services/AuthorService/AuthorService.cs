using BookStoreAPI.Data.Entities;
using BookStoreAPI.Services.AuthorService.DTOs;
using BookStoreAPI.Services.AuthorService.Interfaces;
using BookStoreAPI.Services.AuthorService.Repositories;

namespace BookStoreAPI.Services.AuthorService
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<IEnumerable<AuthorDTO>> GetAllAsync ()
        {
            var authors = await _authorRepository.GetAllAsync();

            return authors.Select(a => new AuthorDTO
            {
                Id = a.Id,
                Name = a.Name,
                IsDeleted = a.IsDeleted
            });
        }

        public async Task<AuthorDTO?> GetByIdAsync(Guid id)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
                throw new KeyNotFoundException($"Author with id {id} not found.");

            return new AuthorDTO
            {
                Id = author.Id,
                Name = author.Name,
                IsDeleted = author.IsDeleted
            };
        }

        public async Task<IEnumerable<AuthorDTO>> SearchByKeywordAsync(string keyword)
        {
            var authors = await _authorRepository.SearchByKeywordAsync(keyword);

            return authors.Select(a => new AuthorDTO
            {
                Id = a.Id,
                Name = a.Name,
                IsDeleted = a.IsDeleted
            });
        }

        public async Task<bool> AddAsync (AuthorCreateDTO authorCreateDTO)
        {
            var author = new Author
            {
                Name = authorCreateDTO.Name
            };

            await _authorRepository.AddAsync(author);

            return await _authorRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync (Guid id, AuthorUpdateDTO authorUpdateDTO)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
                throw new KeyNotFoundException($"Author with id {id} not found.");

            author.Name = authorUpdateDTO.Name;
            author.IsDeleted = authorUpdateDTO.IsDeleted;

            _authorRepository.Update(author);

            return await _authorRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
                throw new KeyNotFoundException($"Author with id {id} not found.");

            if (author.IsDeleted)
                throw new InvalidOperationException($"Author with id {id} is already deleted.");

            _authorRepository.Delete(author);

            return await _authorRepository.SaveChangesAsync();
        }
    }
}
