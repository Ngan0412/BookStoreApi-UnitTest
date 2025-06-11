using BookStoreAPI.Common.Controllers;
using BookStoreAPI.Services.PublisherService.DTOs;
using BookStoreAPI.Services.PublisherService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers
{
    public class PublisherController : BaseController
    {
        private readonly IPublisherService _publisherService;

        public PublisherController(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _publisherService.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _publisherService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchByKeyword([FromQuery] string? keyword)
        {
            var result = await _publisherService.SearchByKeywordAsync(keyword);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] PublisherCreateDTO publisherCreateDTO)
        {
            var result = await _publisherService.AddAsync(publisherCreateDTO);

            return Ok(new { message = "Publisher added successfully." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PublisherUpdateDTO publisherUpdateDTO)
        {
            var result = await _publisherService.UpdateAsync(id, publisherUpdateDTO);

            return Ok(new { message = "Publisher updated successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _publisherService.DeleteAsync(id);

            return Ok(new { message = "Publisher deleted successfully." });
        }
    }
}
