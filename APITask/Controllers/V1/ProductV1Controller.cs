using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using APITask.Models.DTOs;
using APITask.Services;

namespace APITask.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/product")]
    [Tags("Products")]
    public class ProductV1Controller : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductV1Controller(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Get all products with pagination and response caching - V1
        /// </summary>
        [HttpGet]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<IEnumerable<ProductReadDTO>>> GetAllProducts(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 5)
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 5;

                var products = await _productService.GetAllProductsAsync(page, pageSize);
                var totalCount = await _productService.GetTotalCountAsync();

                Response.Headers["X-Total-Count"] = totalCount.ToString();
                Response.Headers["X-Page"] = page.ToString();
                Response.Headers["X-Page-Size"] = pageSize.ToString();

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get product by ID - V1
        /// </summary>
        [HttpGet("{id}")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ProductReadDTO>> GetProduct(int id)
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

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Create a new product - V1
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ProductReadDTO>> CreateProduct([FromBody] ProductCreateDTO productDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdProduct = await _productService.CreateProductAsync(productDto);
                return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Update an existing product - V1
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductReadDTO>> UpdateProduct(int id, [FromBody] ProductUpdateDTO productDto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid product ID");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedProduct = await _productService.UpdateProductAsync(id, productDto);
                if (updatedProduct == null)
                {
                    return NotFound($"Product with ID {id} not found");
                }

                return Ok(updatedProduct);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Partially update a product using JSON Patch - V1
        /// </summary>
        [HttpPatch("{id}")]
        public async Task<ActionResult<ProductReadDTO>> PatchProduct(int id, [FromBody] JsonPatchDocument<ProductUpdateDTO> patchDoc)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid product ID");
                }

                if (patchDoc == null)
                {
                    return BadRequest("Patch document is null");
                }

                var updatedProduct = await _productService.PatchProductAsync(id, patchDoc);
                if (updatedProduct == null)
                {
                    return NotFound($"Product with ID {id} not found");
                }

                return Ok(updatedProduct);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Delete a product - V1
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid product ID");
                }

                var result = await _productService.DeleteProductAsync(id);
                if (!result)
                {
                    return NotFound($"Product with ID {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get products by category - V1
        /// </summary>
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<ProductReadDTO>>> GetProductsByCategory(int categoryId)
        {
            try
            {
                if (categoryId <= 0)
                {
                    return BadRequest("Invalid category ID");
                }

                var products = await _productService.GetProductsByCategoryAsync(categoryId);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
