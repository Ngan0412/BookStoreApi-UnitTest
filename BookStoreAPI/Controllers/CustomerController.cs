using BookStoreAPI.Common.Controllers;
using BookStoreAPI.Services.CustomerSevice.DTOs;
using BookStoreAPI.Services.CustomerSevice.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _customerService.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _customerService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchByKeyword([FromQuery] string? keyword)
        {
            var result = await _customerService.SearchByKeywordAsync(keyword);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CustomerCreateDTO customerCreateDTO)
        {
            var result = await _customerService.AddAsync(customerCreateDTO);

            return Ok(new { message = "Customer added successfully." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CustomerUpdateDTO customerUpdateDTO)
        {
            var result = await _customerService.UpdateAsync(id, customerUpdateDTO);

            return Ok(new { message = "Customer updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _customerService.DeleteAsync(id);

            return Ok(new { message = "Customer deleted successfully" });
        }
    }
}
