using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using APITask.Models.DTOs;
using APITask.Models.Entities;
using APITask.Repositories;

namespace APITask.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductReadDTO>> GetAllProductsAsync(int page = 1, int pageSize = 5)
        {
            var (products, _) = await _unitOfWork.Products.GetPagedWithCategoryAsync(page, pageSize);
            return _mapper.Map<IEnumerable<ProductReadDTO>>(products);
        }

        public async Task<ProductReadDTO?> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdWithCategoryAsync(id);
            if (product == null) return null;

            return _mapper.Map<ProductReadDTO>(product);
        }

        public async Task<ProductReadDTO> CreateProductAsync(ProductCreateDTO productDto)
        {
            // Validate that the category exists
            var categoryExists = await _unitOfWork.Categories.ExistsAsync(productDto.CategoryId);
            if (!categoryExists)
            {
                throw new InvalidOperationException($"Category with ID {productDto.CategoryId} does not exist.");
            }

            // Validate price range
            if (productDto.BookPrice < 1 || productDto.BookPrice > 1000)
            {
                throw new ArgumentException("Book price must be between 1 and 1000.");
            }

            var product = _mapper.Map<Product>(productDto);
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            // Fetch the created product with category for the response
            var createdProduct = await _unitOfWork.Products.GetByIdWithCategoryAsync(product.Id);
            return _mapper.Map<ProductReadDTO>(createdProduct!);
        }

        public async Task<ProductReadDTO?> UpdateProductAsync(int id, ProductUpdateDTO productDto)
        {
            var existingProduct = await _unitOfWork.Products.GetByIdAsync(id);
            if (existingProduct == null) return null;

            // Validate that the category exists
            var categoryExists = await _unitOfWork.Categories.ExistsAsync(productDto.CategoryId);
            if (!categoryExists)
            {
                throw new InvalidOperationException($"Category with ID {productDto.CategoryId} does not exist.");
            }

            // Validate price range
            if (productDto.BookPrice < 1 || productDto.BookPrice > 1000)
            {
                throw new ArgumentException("Book price must be between 1 and 1000.");
            }

            _mapper.Map(productDto, existingProduct);
            await _unitOfWork.Products.UpdateAsync(existingProduct);
            await _unitOfWork.SaveChangesAsync();

            // Fetch the updated product with category for the response
            var updatedProduct = await _unitOfWork.Products.GetByIdWithCategoryAsync(id);
            return _mapper.Map<ProductReadDTO>(updatedProduct!);
        }

        public async Task<ProductReadDTO?> PatchProductAsync(int id, JsonPatchDocument<ProductUpdateDTO> patchDoc)
        {
            var existingProduct = await _unitOfWork.Products.GetByIdAsync(id);
            if (existingProduct == null) return null;

            var productToPatch = _mapper.Map<ProductUpdateDTO>(existingProduct);
            patchDoc.ApplyTo(productToPatch);

            // Validate that the category exists if CategoryId was changed
            var categoryExists = await _unitOfWork.Categories.ExistsAsync(productToPatch.CategoryId);
            if (!categoryExists)
            {
                throw new InvalidOperationException($"Category with ID {productToPatch.CategoryId} does not exist.");
            }

            // Validate price range
            if (productToPatch.BookPrice < 1 || productToPatch.BookPrice > 1000)
            {
                throw new ArgumentException("Book price must be between 1 and 1000.");
            }

            _mapper.Map(productToPatch, existingProduct);
            await _unitOfWork.Products.UpdateAsync(existingProduct);
            await _unitOfWork.SaveChangesAsync();

            // Fetch the updated product with category for the response
            var updatedProduct = await _unitOfWork.Products.GetByIdWithCategoryAsync(id);
            return _mapper.Map<ProductReadDTO>(updatedProduct!);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null) return false;

            await _unitOfWork.Products.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<ProductReadDTO>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _unitOfWork.Products.GetByCategoryIdAsync(categoryId);
            return _mapper.Map<IEnumerable<ProductReadDTO>>(products);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _unitOfWork.Products.CountAsync();
        }
    }
}
