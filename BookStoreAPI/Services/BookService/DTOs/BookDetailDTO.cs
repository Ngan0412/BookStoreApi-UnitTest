namespace BookStoreAPI.Services.BookService.DTOs
{
    public class BookDetailDTO
    {
        public Guid Id { get; set; }
        public string Isbn { get; set; }
        public string Title { get; set; }
        public string CategoryName { get; set; }
        public string AuthorName { get; set; }
        public string PublisherName { get; set; }
        public Guid CategoryId { get; set; }
        public Guid AuthorId { get; set; }
        public Guid PublisherId { get; set; }
        public short YearOfPublication { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public bool IsDeleted { get; set; }
    }

}
