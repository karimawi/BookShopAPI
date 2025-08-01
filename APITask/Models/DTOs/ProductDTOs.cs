namespace APITask.Models.DTOs
{
    public class ProductReadDTO
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public required string Author { get; set; }
        public decimal BookPrice { get; set; }
        public int CategoryId { get; set; }
        public required string CategoryName { get; set; }
    }

    public class ProductCreateDTO
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public required string Author { get; set; }
        public decimal BookPrice { get; set; }
        public int CategoryId { get; set; }
    }

    public class ProductUpdateDTO
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public required string Author { get; set; }
        public decimal BookPrice { get; set; }
        public int CategoryId { get; set; }
    }
}
