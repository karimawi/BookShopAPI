namespace APITask.Models.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public required string Author { get; set; }
        public decimal BookPrice { get; set; }
        public bool IsDeleted { get; set; }

        // Foreign key
        public int CategoryId { get; set; }
        
        // Navigation property
        public virtual Category? Category { get; set; }

        public Product()
        {
            IsDeleted = false;
        }
    }
}
