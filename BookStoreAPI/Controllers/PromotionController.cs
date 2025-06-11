using BookStoreAPI.Common.Controllers;
using BookStoreAPI.Services.PromotionService.DTOs;
using BookStoreAPI.Services.PromotionService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers
{
    public class PromotionController : BaseController
    {
        private readonly IPromotionService _promotionService;

        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _promotionService.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _promotionService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchByKeyword([FromQuery] string? keyword)
        {
            var result = await _promotionService.SearchByKeywordAsync(keyword);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] PromotionCreateDTO promotionCreateDTO)
        {
            var result = await _promotionService.AddAsync(promotionCreateDTO);

            return Ok(new { message = "Promotion created successfully." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PromotionUpdateDTO promotionUpdateDTO)
        {
            var result = await _promotionService.UpdateAsync(id, promotionUpdateDTO);

            return Ok(new { message = "Promotion updated successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _promotionService.DeleteAsync(id);

            return Ok(new { message = "Promotion soft deleted successfully." });
        }
    }
}
