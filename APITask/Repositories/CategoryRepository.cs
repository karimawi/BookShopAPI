using Microsoft.EntityFrameworkCore;
using APITask.DataAccess;
using APITask.Models.Entities;

namespace APITask.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<(IEnumerable<Category> Items, int TotalCount)> GetPagedSortedAsync(int page, int pageSize)
        {
            var totalCount = await CountAsync();
            var items = await _dbSet
                .OrderBy(c => c.CatOrder)
                .ThenBy(c => c.CatName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<bool> CategoryNameExistsAsync(string catName, int? excludeId = null)
        {
            return await _dbSet.AnyAsync(c => c.CatName == catName && (excludeId == null || c.Id != excludeId));
        }
    }
}
