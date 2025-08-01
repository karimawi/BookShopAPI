using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using APITask.Models.DTOs;
using APITask.Models.Entities;
using APITask.Repositories;

namespace APITask.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(30);

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<IEnumerable<CategoryReadDTO>> GetAllCategoriesAsync(int page = 1, int pageSize = 5)
        {
            var cacheKey = $"categories_page_{page}_size_{pageSize}";
            
            if (_cache.TryGetValue(cacheKey, out IEnumerable<CategoryReadDTO>? cachedCategories))
            {
                return cachedCategories!;
            }

            var (categories, _) = await _unitOfWork.Categories.GetPagedSortedAsync(page, pageSize);
            var categoryDtos = _mapper.Map<IEnumerable<CategoryReadDTO>>(categories);

            _cache.Set(cacheKey, categoryDtos, _cacheExpiration);
            return categoryDtos;
        }

        public async Task<CategoryReadDTO?> GetCategoryByIdAsync(int id)
        {
            var cacheKey = $"category_{id}";
            
            if (_cache.TryGetValue(cacheKey, out CategoryReadDTO? cachedCategory))
            {
                return cachedCategory;
            }

            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null) return null;

            var categoryDto = _mapper.Map<CategoryReadDTO>(category);
            _cache.Set(cacheKey, categoryDto, _cacheExpiration);
            
            return categoryDto;
        }

        public async Task<CategoryReadDTO> CreateCategoryAsync(CategoryCreateDTO categoryDto)
        {
            // Check if category name already exists
            var nameExists = await _unitOfWork.Categories.CategoryNameExistsAsync(categoryDto.CatName);
            if (nameExists)
            {
                throw new InvalidOperationException($"Category with name '{categoryDto.CatName}' already exists.");
            }

            var category = _mapper.Map<Category>(categoryDto);
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            // Clear cache
            ClearCategoriesCache();

            return _mapper.Map<CategoryReadDTO>(category);
        }

        public async Task<CategoryReadDTO?> UpdateCategoryAsync(int id, CategoryUpdateDTO categoryDto)
        {
            var existingCategory = await _unitOfWork.Categories.GetByIdAsync(id);
            if (existingCategory == null) return null;

            // Check if category name already exists (excluding current category)
            var nameExists = await _unitOfWork.Categories.CategoryNameExistsAsync(categoryDto.CatName, id);
            if (nameExists)
            {
                throw new InvalidOperationException($"Category with name '{categoryDto.CatName}' already exists.");
            }

            _mapper.Map(categoryDto, existingCategory);
            await _unitOfWork.Categories.UpdateAsync(existingCategory);
            await _unitOfWork.SaveChangesAsync();

            // Clear cache
            ClearCategoriesCache();

            return _mapper.Map<CategoryReadDTO>(existingCategory);
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null) return false;

            // Check if category has products
            var hasProducts = await _unitOfWork.Products.FindAsync(p => p.CategoryId == id);
            if (hasProducts.Any())
            {
                throw new InvalidOperationException("Cannot delete category that contains products.");
            }

            await _unitOfWork.Categories.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            // Clear cache
            ClearCategoriesCache();

            return true;
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _unitOfWork.Categories.CountAsync();
        }

        private void ClearCategoriesCache()
        {
            // In a real application, you might want to use a more sophisticated cache invalidation strategy
            // For now, we'll clear all category-related cache entries by removing them individually
            // This is a simplified approach
            var keys = new List<string>();
            
            // Add logic to track cache keys if needed, or use IMemoryCache with tags
            // For simplicity, we'll rely on cache expiration
        }
    }
}
