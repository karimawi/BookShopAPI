using APITask.Models.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace APITask.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductReadDTO>> GetAllProductsAsync(int page = 1, int pageSize = 5);
        Task<ProductReadDTO?> GetProductByIdAsync(int id);
        Task<ProductReadDTO> CreateProductAsync(ProductCreateDTO productDto);
        Task<ProductReadDTO?> UpdateProductAsync(int id, ProductUpdateDTO productDto);
        Task<ProductReadDTO?> PatchProductAsync(int id, JsonPatchDocument<ProductUpdateDTO> patchDoc);
        Task<bool> DeleteProductAsync(int id);
        Task<IEnumerable<ProductReadDTO>> GetProductsByCategoryAsync(int categoryId);
        Task<int> GetTotalCountAsync();
    }
}
