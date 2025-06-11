namespace BookStoreAPI.Services.OrderService.DTOs
{
    public class OrderCreateDTO
    {
        public  Guid CustomerId { get; set; }
        public Guid? PromotionId { get; set; }
        public List<OrderItemCreateDTO> Items { get; set; } = new();
    }
}
