using APITask.Models.DTOs;

namespace APITask.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryReadDTO>> GetAllCategoriesAsync(int page = 1, int pageSize = 5);
        Task<CategoryReadDTO?> GetCategoryByIdAsync(int id);
        Task<CategoryReadDTO> CreateCategoryAsync(CategoryCreateDTO categoryDto);
        Task<CategoryReadDTO?> UpdateCategoryAsync(int id, CategoryUpdateDTO categoryDto);
        Task<bool> DeleteCategoryAsync(int id);
        Task<int> GetTotalCountAsync();
    }
}
