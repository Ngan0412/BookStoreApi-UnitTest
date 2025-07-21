using BookStoreAPI.Common.Controllers;
using BookStoreAPI.Services.BookService.DTOs;
using BookStoreAPI.Services.BookService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers
{
    public class BookController : BaseController
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaging(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _bookService.GetPagedAsync(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _bookService.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("GetByCategory")]
        public async Task<IActionResult> GetByCategory(Guid categoryId)
        {
            var result = await _bookService.GetAllAsync();
            var filteredBooks = result.Where(book => book.CategoryId == categoryId).ToList();
            return Ok(filteredBooks);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _bookService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchByKeyword([FromQuery] string? keyword)
        {
            var result = await _bookService.SearchByKeywordAsync(keyword);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] BookCreateDTO bookCreateDTO)
        {
            var result = await _bookService.AddAsync(bookCreateDTO);

            return Ok(new { message = "Book added successfully." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] BookUpdateDTO bookUpdateDTO)
        {
            var result = await _bookService.UpdateAsync(id, bookUpdateDTO);

            return Ok(new { message = "Book updated successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _bookService.DeleteAsync(id);

            return Ok(new { message = "Book soft deleted successfully." });
        }
    }
}
