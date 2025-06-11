using BookStoreAPI.Data;
using BookStoreAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookStoreAPI.Services.OrderService.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.Staff)
                .Include(o => o.Customer)
                .Include(o => o.Promotion)
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Book)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            return await _context.Orders
                .Include(o => o.Staff)
                .Include(o => o.Customer)
                .Include(o => o.Promotion)
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Book)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<OrderItem>> GetItemsByOrderIdAsync(Guid orderId)
        {
            return await _context.OrderItems
                .Include(oi => oi.Book)
                .Where(oi => oi.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<OrderItem?> GetOrderItemByOrderIdAndBookIdAsync(Guid orderId, Guid bookId)
        {
            return await _context.OrderItems
                .FirstOrDefaultAsync(oi => oi.OrderId == orderId && oi.BookId == bookId);
        }

        public async Task<Book?> GetBookByIdAsync(Guid id)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.Id == id && b.IsDeleted == false);
        }

        public async Task<Promotion?> GetPromotionByIdAsync(Guid id)
        {
            return await _context.Promotions.FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted == false);
        }

        public async Task<Customer?> GetCustomerByIdAsync(Guid id)
        {
            return await _context.Customers.FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted == false);
        }

        public async Task<IEnumerable<Order>> SearchByKeywordAsync(string keyword)
        {
            return string.IsNullOrWhiteSpace(keyword)
                ? await _context.Orders.ToListAsync()
                : await _context.Orders
                .Include(o => o.Staff)
                .Include(o => o.Customer)
                .Include(o => o.Promotion)
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Book)
                .Where(o => ((o.Staff.FamilyName + ' ' + o.Staff.GivenName).Contains(keyword))
                || ((o.Customer.FamilyName + ' ' + o.Customer.GivenName).Contains(keyword))
                || (o.Promotion != null && o.Promotion.Name.Contains(keyword))
                || (o.Note != null && o.Note.Contains(keyword)))
                .ToListAsync();
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public void Update(Order order)
        {
            _context.Orders.Update(order);
        }

        public void Delete(Order order)
        {
            order.IsDeleted = true;
            _context.Orders.Update(order);
        }

        public void DeleteItem(OrderItem orderItem)
        {
            orderItem.IsDeleted = true;

            _context.OrderItems.Update(orderItem);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
