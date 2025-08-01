using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using APITask.Models.DTOs;
using APITask.Services;

namespace APITask.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/product")]
    [Tags("Products")]
    public class ProductV2Controller : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductV2Controller(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Get all products with enhanced pagination and filtering - V2
        /// </summary>
        [HttpGet]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<object>> GetAllProducts(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 5,
            [FromQuery] string? search = null)
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 5;

                var products = await _productService.GetAllProductsAsync(page, pageSize);
                var totalCount = await _productService.GetTotalCountAsync();

                // V2 returns enhanced response with metadata
                var response = new
                {
                    Data = products,
                    Pagination = new
                    {
                        Page = page,
                        PageSize = pageSize,
                        TotalCount = totalCount,
                        TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                    },
                    Version = "2.0"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get product by ID with enhanced details - V2
        /// </summary>
        [HttpGet("{id}")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<object>> GetProduct(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid product ID");
                }

                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound($"Product with ID {id} not found");
                }

                // V2 returns enhanced response with metadata
                var response = new
                {
                    Data = product,
                    Version = "2.0",
                    Timestamp = DateTime.UtcNow
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Create a new product with enhanced validation - V2
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<object>> CreateProduct([FromBody] ProductCreateDTO productDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { Error = "Validation failed", Details = ModelState });
                }

                var createdProduct = await _productService.CreateProductAsync(productDto);
                
                var response = new
                {
                    Data = createdProduct,
                    Message = "Product created successfully",
                    Version = "2.0"
                };
                
                return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Error = ex.Message, Version = "2.0" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message, Version = "2.0" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"Internal server error: {ex.Message}", Version = "2.0" });
            }
        }

        /// <summary>
        /// Update an existing product with enhanced validation - V2
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<object>> UpdateProduct(int id, [FromBody] ProductUpdateDTO productDto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { Error = "Invalid product ID", Version = "2.0" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new { Error = "Validation failed", Details = ModelState, Version = "2.0" });
                }

                var updatedProduct = await _productService.UpdateProductAsync(id, productDto);
                if (updatedProduct == null)
                {
                    return NotFound(new { Error = $"Product with ID {id} not found", Version = "2.0" });
                }

                var response = new
                {
                    Data = updatedProduct,
                    Message = "Product updated successfully",
                    Version = "2.0"
                };

                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Error = ex.Message, Version = "2.0" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message, Version = "2.0" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"Internal server error: {ex.Message}", Version = "2.0" });
            }
        }

        /// <summary>
        /// Partially update a product using JSON Patch with enhanced validation - V2
        /// </summary>
        [HttpPatch("{id}")]
        public async Task<ActionResult<object>> PatchProduct(int id, [FromBody] JsonPatchDocument<ProductUpdateDTO> patchDoc)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { Error = "Invalid product ID", Version = "2.0" });
                }

                if (patchDoc == null)
                {
                    return BadRequest(new { Error = "Patch document is null", Version = "2.0" });
                }

                var updatedProduct = await _productService.PatchProductAsync(id, patchDoc);
                if (updatedProduct == null)
                {
                    return NotFound(new { Error = $"Product with ID {id} not found", Version = "2.0" });
                }

                var response = new
                {
                    Data = updatedProduct,
                    Message = "Product patched successfully",
                    Version = "2.0"
                };

                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Error = ex.Message, Version = "2.0" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message, Version = "2.0" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"Internal server error: {ex.Message}", Version = "2.0" });
            }
        }

        /// <summary>
        /// Delete a product - V2
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<object>> DeleteProduct(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { Error = "Invalid product ID", Version = "2.0" });
                }

                var result = await _productService.DeleteProductAsync(id);
                if (!result)
                {
                    return NotFound(new { Error = $"Product with ID {id} not found", Version = "2.0" });
                }

                return Ok(new { Message = "Product deleted successfully", Version = "2.0" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"Internal server error: {ex.Message}", Version = "2.0" });
            }
        }

        /// <summary>
        /// Get products by category with enhanced response - V2
        /// </summary>
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<object>> GetProductsByCategory(int categoryId)
        {
            try
            {
                if (categoryId <= 0)
                {
                    return BadRequest(new { Error = "Invalid category ID", Version = "2.0" });
                }

                var products = await _productService.GetProductsByCategoryAsync(categoryId);
                
                var response = new
                {
                    Data = products,
                    CategoryId = categoryId,
                    Count = products.Count(),
                    Version = "2.0"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"Internal server error: {ex.Message}", Version = "2.0" });
            }
        }
    }
}
