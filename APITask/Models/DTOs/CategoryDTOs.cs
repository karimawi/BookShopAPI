namespace APITask.Models.DTOs
{
    public class CategoryReadDTO
    {
        public int Id { get; set; }
        public required string CatName { get; set; }
        public int CatOrder { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class CategoryCreateDTO
    {
        public required string CatName { get; set; }
        public int CatOrder { get; set; }
    }

    public class CategoryUpdateDTO
    {
        public required string CatName { get; set; }
        public int CatOrder { get; set; }
    }
}
