using BookStoreAPI.Common.Controllers;
using BookStoreAPI.Services.AuthorService.DTOs;
using BookStoreAPI.Services.AuthorService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers
{
    public class AuthorController : BaseController
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var authors = await _authorService.GetAllAsync();

            return Ok(authors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var author = await _authorService.GetByIdAsync(id);

            return Ok(author);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchByKeyword([FromQuery] string? keyword)
        {
            var authors = await _authorService.SearchByKeywordAsync(keyword);

            return Ok(authors);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AuthorCreateDTO authorCreateDTO)
        {
            await _authorService.AddAsync(authorCreateDTO);

            return Ok(new { message = "Author added successfully." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AuthorUpdateDTO authorUpdateDTO)
        {
            await _authorService.UpdateAsync(id, authorUpdateDTO);

            return Ok(new { message = "Author updated successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _authorService.DeleteAsync(id);

            return Ok(new { message = "Author soft deleted successfully." });
        }
    }
}
