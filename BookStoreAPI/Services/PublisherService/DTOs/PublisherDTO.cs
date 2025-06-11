namespace BookStoreAPI.Services.PublisherService.DTOs
{
    public class PublisherDTO
    {
        public Guid Id { get; set; } 
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }
}
