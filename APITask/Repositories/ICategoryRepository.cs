using APITask.Models.Entities;

namespace APITask.Repositories
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<(IEnumerable<Category> Items, int TotalCount)> GetPagedSortedAsync(int page, int pageSize);
        Task<bool> CategoryNameExistsAsync(string catName, int? excludeId = null);
    }
}
