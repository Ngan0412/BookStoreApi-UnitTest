using BookStoreAPI.Data.Entities;

namespace BookStoreAPI.Services.OrderService.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(Guid id);
        Task<IEnumerable<OrderItem>> GetItemsByOrderIdAsync(Guid orderId);
        Task<OrderItem?> GetOrderItemByOrderIdAndBookIdAsync(Guid orderId, Guid bookId);
        Task<Book?> GetBookByIdAsync(Guid id);
        Task<Promotion?> GetPromotionByIdAsync(Guid id);
        Task<Customer?> GetCustomerByIdAsync(Guid id);
        Task<IEnumerable<Order>> SearchByKeywordAsync(string keyword);
        Task AddAsync(Order order);
        void Update(Order order);
        void Delete(Order order);
        void DeleteItem(OrderItem item);
        Task<bool> SaveChangesAsync();
    }
}
