using BookStoreAPI.Common.Controllers;
using BookStoreAPI.Services.OrderService.DTOs;
using BookStoreAPI.Services.OrderService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _orderService.GetAllAsync(User);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _orderService.GetByIdAsync(id, User);

            return Ok(result);
        }

        [HttpGet("items/{orderId}")]
        public async Task<IActionResult> GetItemsByOrderId(Guid orderId)
        {
            var result = await _orderService.GetItemsByOrderIdAsync(orderId, User);

            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchByKeyword([FromQuery] string? keyword)
        {
            var result = await _orderService.SearchByKeywordAsync(keyword, User);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] OrderCreateDTO orderCreateDTO)
        {
            var result = await _orderService.AddAsync(orderCreateDTO, User);

            return Ok(new { message = "Order created successfully." });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] OrderUpdateDTO orderUpdateDTO)
        {
            var result = await _orderService.UpdateAsync(id, orderUpdateDTO);

            return Ok(new { message = "Order updated successfully." });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _orderService.DeleteAsync(id);

            return Ok(new { message = "Order soft deleted successfully." });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("items/{orderId}/{bookId}")]
        public async Task<IActionResult> DeleteItem(Guid orderId, Guid bookId)
        {
            var result = await _orderService.DeleteItem(orderId, bookId);
            return Ok(new { message = "Order item soft deleted successfully." });
        }
    }
}
