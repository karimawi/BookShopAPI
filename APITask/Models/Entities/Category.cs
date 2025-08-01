namespace APITask.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public required string CatName { get; set; }
        public int CatOrder { get; set; }
        
        public DateTime CreatedDate { get; set; }
        
        public bool IsDeleted { get; set; }

        // Navigation property
        public virtual ICollection<Product> Products { get; set; }

        public Category()
        {
            Products = new HashSet<Product>();
            CreatedDate = DateTime.UtcNow;
            IsDeleted = false;
        }
    }
}
