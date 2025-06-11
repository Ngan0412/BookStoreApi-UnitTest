namespace BookStoreAPI.Services.OrderService.DTOs
{
    public class OrderItemDTO
    {
        public Guid OrderId { get; set; }
        public Guid BookId { get; set; }
        public string BookName { get; set; } = null!;
        public short Quantity { get; set; }
        public decimal Price { get; set; }
        public bool IsDeleted { get; set; }
    }
}
