using BookStoreAPI.Common.Controllers;
using BookStoreAPI.Services.CategoryService.DTOs;
using BookStoreAPI.Services.CategoryService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _categoryService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchByKeyword([FromQuery] string? keyword)
        {
            var result = await _categoryService.SearchByKeywordAsync(keyword);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CategoryCreateDTO categoryCreateDTO)
        {
            await _categoryService.AddAsync(categoryCreateDTO);
            return Ok(new { message = "Category added successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CategoryUpdateDTO categoryUpdateDTO)
        {
            await _categoryService.UpdateAsync(id, categoryUpdateDTO);
            return Ok(new { message = "Category updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _categoryService.DeleteAsync(id);
            return Ok(new { message = "Category soft deleted successfully" });
        }
    }
}
