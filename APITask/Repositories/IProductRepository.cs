using APITask.Models.Entities;

namespace APITask.Repositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllWithCategoryAsync();
        Task<Product?> GetByIdWithCategoryAsync(int id);
        Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedWithCategoryAsync(int page, int pageSize);
        Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId);
    }
}
