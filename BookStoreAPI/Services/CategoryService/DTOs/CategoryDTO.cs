namespace BookStoreAPI.Services.CategoryService.DTOs
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }
    }
}
