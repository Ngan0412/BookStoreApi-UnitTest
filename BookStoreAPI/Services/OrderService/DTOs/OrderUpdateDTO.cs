namespace BookStoreAPI.Services.OrderService.DTOs
{
    public class OrderUpdateDTO
    {
        public bool Status { get; set; }
        public string? Note { get; set; }
        public bool IsDeleted { get; set; }
    }
}
