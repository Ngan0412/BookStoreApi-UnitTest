using BookStoreAPI.Services.OrderService.DTOs;
using System.Security.Claims;

namespace BookStoreAPI.Services.OrderService.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetAllAsync(ClaimsPrincipal user);
        Task<OrderDTO?> GetByIdAsync(Guid id, ClaimsPrincipal user);
        Task<IEnumerable<OrderItemDTO>> GetItemsByOrderIdAsync(Guid orderId, ClaimsPrincipal user);
        Task<IEnumerable<OrderDTO>> SearchByKeywordAsync(string keyword, ClaimsPrincipal user);
        Task<bool> AddAsync(OrderCreateDTO orderCreateDTO, ClaimsPrincipal user);
        Task<bool> UpdateAsync(Guid id, OrderUpdateDTO orderUpdateDTO);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> DeleteItem(Guid orderId, Guid bookId);
    }
}
