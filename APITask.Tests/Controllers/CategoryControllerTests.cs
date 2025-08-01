using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;
using FluentAssertions;
using AutoFixture;
using AutoFixture.Xunit2;
using APITask.Controllers;
using APITask.Models.DTOs;
using APITask.Services;

namespace APITask.Tests.Controllers
{
    public class CategoryControllerTests
    {
        private readonly Mock<ICategoryService> _mockCategoryService;
        private readonly CategoryController _controller;
        private readonly IFixture _fixture;

        public CategoryControllerTests()
        {
            _mockCategoryService = new Mock<ICategoryService>();
            _controller = new CategoryController(_mockCategoryService.Object);
            _fixture = new Fixture();

            // Setup controller context for header testing
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        #region GetCategories Tests

        [Fact]
        public async Task GetCategories_WithValidParameters_ReturnsOkResultWithCategories()
        {
             
            var page = 1;
            var pageSize = 5;
            var totalCount = 10;
            var categories = _fixture.CreateMany<CategoryReadDTO>(3).ToList();

            _mockCategoryService.Setup(s => s.GetAllCategoriesAsync(page, pageSize))
                .ReturnsAsync(categories);
            _mockCategoryService.Setup(s => s.GetTotalCountAsync())
                .ReturnsAsync(totalCount);

            // Act
            var result = await _controller.GetCategories(page, pageSize);

             
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(categories);
            
            // Verify headers are set
            _controller.Response.Headers["X-Total-Count"].ToString().Should().Be(totalCount.ToString());
            _controller.Response.Headers["X-Page"].ToString().Should().Be(page.ToString());
            _controller.Response.Headers["X-Page-Size"].ToString().Should().Be(pageSize.ToString());

            _mockCategoryService.Verify(s => s.GetAllCategoriesAsync(page, pageSize), Times.Once);
            _mockCategoryService.Verify(s => s.GetTotalCountAsync(), Times.Once);
        }

        [Theory]
        [InlineData(0, 5)]
        [InlineData(-1, 5)]
        [InlineData(1, 0)]
        [InlineData(1, -1)]
        public async Task GetCategories_WithInvalidParameters_FixesParametersAndReturnsCategories(int page, int pageSize)
        {
             
            var expectedPage = page < 1 ? 1 : page;
            var expectedPageSize = pageSize < 1 ? 5 : pageSize;
            var categories = _fixture.CreateMany<CategoryReadDTO>(2).ToList();

            _mockCategoryService.Setup(s => s.GetAllCategoriesAsync(expectedPage, expectedPageSize))
                .ReturnsAsync(categories);
            _mockCategoryService.Setup(s => s.GetTotalCountAsync())
                .ReturnsAsync(5);

            // Act
            var result = await _controller.GetCategories(page, pageSize);

             
            result.Result.Should().BeOfType<OkObjectResult>();
            _mockCategoryService.Verify(s => s.GetAllCategoriesAsync(expectedPage, expectedPageSize), Times.Once);
        }

        [Fact]
        public async Task GetCategories_WhenServiceThrowsException_ReturnsInternalServerError()
        {
             
            var exceptionMessage = "Database connection failed";
            _mockCategoryService.Setup(s => s.GetAllCategoriesAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.GetCategories();

             
            var statusCodeResult = result.Result.Should().BeOfType<ObjectResult>().Subject;
            statusCodeResult.StatusCode.Should().Be(500);
            statusCodeResult.Value.Should().Be($"Internal server error: {exceptionMessage}");
        }

        #endregion

        #region GetCategory Tests

        [Fact]
        public async Task GetCategory_WithValidId_ReturnsOkResultWithCategory()
        {
             
            var categoryId = 1;
            var category = _fixture.Create<CategoryReadDTO>();
            category.Id = categoryId;

            _mockCategoryService.Setup(s => s.GetCategoryByIdAsync(categoryId))
                .ReturnsAsync(category);

            // Act
            var result = await _controller.GetCategory(categoryId);

             
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(category);
            _mockCategoryService.Verify(s => s.GetCategoryByIdAsync(categoryId), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public async Task GetCategory_WithInvalidId_ReturnsBadRequest(int invalidId)
        {
            // Act
            var result = await _controller.GetCategory(invalidId);

             
            var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().Be("Invalid category ID");
            _mockCategoryService.Verify(s => s.GetCategoryByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetCategory_WhenCategoryNotFound_ReturnsNotFound()
        {
             
            var categoryId = 999;
            _mockCategoryService.Setup(s => s.GetCategoryByIdAsync(categoryId))
                .ReturnsAsync((CategoryReadDTO?)null);

            // Act
            var result = await _controller.GetCategory(categoryId);

             
            var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
            notFoundResult.Value.Should().Be($"Category with ID {categoryId} not found");
        }

        [Fact]
        public async Task GetCategory_WhenServiceThrowsException_ReturnsInternalServerError()
        {
             
            var categoryId = 1;
            var exceptionMessage = "Database error";
            _mockCategoryService.Setup(s => s.GetCategoryByIdAsync(categoryId))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.GetCategory(categoryId);

             
            var statusCodeResult = result.Result.Should().BeOfType<ObjectResult>().Subject;
            statusCodeResult.StatusCode.Should().Be(500);
            statusCodeResult.Value.Should().Be($"Internal server error: {exceptionMessage}");
        }

        #endregion

        #region CreateCategory Tests

        [Fact]
        public async Task CreateCategory_WithValidCategory_ReturnsCreatedAtActionResult()
        {
             
            var categoryCreateDto = _fixture.Create<CategoryCreateDTO>();
            var createdCategory = _fixture.Create<CategoryReadDTO>();

            _mockCategoryService.Setup(s => s.CreateCategoryAsync(categoryCreateDto))
                .ReturnsAsync(createdCategory);

            // Act
            var result = await _controller.CreateCategory(categoryCreateDto);

             
            var createdAtActionResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdAtActionResult.ActionName.Should().Be(nameof(CategoryController.GetCategory));
            createdAtActionResult.RouteValues!["id"].Should().Be(createdCategory.Id);
            createdAtActionResult.Value.Should().BeEquivalentTo(createdCategory);
            
            _mockCategoryService.Verify(s => s.CreateCategoryAsync(categoryCreateDto), Times.Once);
        }

        [Fact]
        public async Task CreateCategory_WithInvalidModelState_ReturnsBadRequest()
        {
             
            var categoryCreateDto = _fixture.Create<CategoryCreateDTO>();
            _controller.ModelState.AddModelError("CatName", "Category name is required");

            // Act
            var result = await _controller.CreateCategory(categoryCreateDto);

             
            var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeOfType<SerializableError>();
            _mockCategoryService.Verify(s => s.CreateCategoryAsync(It.IsAny<CategoryCreateDTO>()), Times.Never);
        }

        [Fact]
        public async Task CreateCategory_WhenCategoryNameAlreadyExists_ReturnsConflict()
        {
             
            var categoryCreateDto = _fixture.Create<CategoryCreateDTO>();
            var conflictMessage = $"Category with name '{categoryCreateDto.CatName}' already exists.";

            _mockCategoryService.Setup(s => s.CreateCategoryAsync(categoryCreateDto))
                .ThrowsAsync(new InvalidOperationException(conflictMessage));

            // Act
            var result = await _controller.CreateCategory(categoryCreateDto);

             
            var conflictResult = result.Result.Should().BeOfType<ConflictObjectResult>().Subject;
            conflictResult.Value.Should().Be(conflictMessage);
        }

        [Fact]
        public async Task CreateCategory_WhenServiceThrowsException_ReturnsInternalServerError()
        {
             
            var categoryCreateDto = _fixture.Create<CategoryCreateDTO>();
            var exceptionMessage = "Database connection failed";

            _mockCategoryService.Setup(s => s.CreateCategoryAsync(categoryCreateDto))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.CreateCategory(categoryCreateDto);

             
            var statusCodeResult = result.Result.Should().BeOfType<ObjectResult>().Subject;
            statusCodeResult.StatusCode.Should().Be(500);
            statusCodeResult.Value.Should().Be($"Internal server error: {exceptionMessage}");
        }

        #endregion

        #region UpdateCategory Tests

        [Fact]
        public async Task UpdateCategory_WithValidData_ReturnsOkResultWithUpdatedCategory()
        {
             
            var categoryId = 1;
            var categoryUpdateDto = _fixture.Create<CategoryUpdateDTO>();
            var updatedCategory = _fixture.Create<CategoryReadDTO>();

            _mockCategoryService.Setup(s => s.UpdateCategoryAsync(categoryId, categoryUpdateDto))
                .ReturnsAsync(updatedCategory);

            // Act
            var result = await _controller.UpdateCategory(categoryId, categoryUpdateDto);

             
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(updatedCategory);
            _mockCategoryService.Verify(s => s.UpdateCategoryAsync(categoryId, categoryUpdateDto), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task UpdateCategory_WithInvalidId_ReturnsBadRequest(int invalidId)
        {
             
            var categoryUpdateDto = _fixture.Create<CategoryUpdateDTO>();

            // Act
            var result = await _controller.UpdateCategory(invalidId, categoryUpdateDto);

             
            var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().Be("Invalid category ID");
            _mockCategoryService.Verify(s => s.UpdateCategoryAsync(It.IsAny<int>(), It.IsAny<CategoryUpdateDTO>()), Times.Never);
        }

        [Fact]
        public async Task UpdateCategory_WithInvalidModelState_ReturnsBadRequest()
        {
             
            var categoryId = 1;
            var categoryUpdateDto = _fixture.Create<CategoryUpdateDTO>();
            _controller.ModelState.AddModelError("CatName", "Category name is required");

            // Act
            var result = await _controller.UpdateCategory(categoryId, categoryUpdateDto);

             
            var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeOfType<SerializableError>();
            _mockCategoryService.Verify(s => s.UpdateCategoryAsync(It.IsAny<int>(), It.IsAny<CategoryUpdateDTO>()), Times.Never);
        }

        [Fact]
        public async Task UpdateCategory_WhenCategoryNotFound_ReturnsNotFound()
        {
             
            var categoryId = 999;
            var categoryUpdateDto = _fixture.Create<CategoryUpdateDTO>();

            _mockCategoryService.Setup(s => s.UpdateCategoryAsync(categoryId, categoryUpdateDto))
                .ReturnsAsync((CategoryReadDTO?)null);

            // Act
            var result = await _controller.UpdateCategory(categoryId, categoryUpdateDto);

             
            var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
            notFoundResult.Value.Should().Be($"Category with ID {categoryId} not found");
        }

        [Fact]
        public async Task UpdateCategory_WhenCategoryNameAlreadyExists_ReturnsConflict()
        {
             
            var categoryId = 1;
            var categoryUpdateDto = _fixture.Create<CategoryUpdateDTO>();
            var conflictMessage = $"Category with name '{categoryUpdateDto.CatName}' already exists.";

            _mockCategoryService.Setup(s => s.UpdateCategoryAsync(categoryId, categoryUpdateDto))
                .ThrowsAsync(new InvalidOperationException(conflictMessage));

            // Act
            var result = await _controller.UpdateCategory(categoryId, categoryUpdateDto);

             
            var conflictResult = result.Result.Should().BeOfType<ConflictObjectResult>().Subject;
            conflictResult.Value.Should().Be(conflictMessage);
        }

        #endregion

        #region DeleteCategory Tests

        [Fact]
        public async Task DeleteCategory_WithValidId_ReturnsNoContent()
        {
             
            var categoryId = 1;
            _mockCategoryService.Setup(s => s.DeleteCategoryAsync(categoryId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteCategory(categoryId);

             
            result.Should().BeOfType<NoContentResult>();
            _mockCategoryService.Verify(s => s.DeleteCategoryAsync(categoryId), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task DeleteCategory_WithInvalidId_ReturnsBadRequest(int invalidId)
        {
            // Act
            var result = await _controller.DeleteCategory(invalidId);

             
            var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().Be("Invalid category ID");
            _mockCategoryService.Verify(s => s.DeleteCategoryAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DeleteCategory_WhenCategoryNotFound_ReturnsNotFound()
        {
             
            var categoryId = 999;
            _mockCategoryService.Setup(s => s.DeleteCategoryAsync(categoryId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteCategory(categoryId);

             
            var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
            notFoundResult.Value.Should().Be($"Category with ID {categoryId} not found");
        }

        [Fact]
        public async Task DeleteCategory_WhenCategoryHasProducts_ReturnsConflict()
        {
             
            var categoryId = 1;
            var conflictMessage = "Cannot delete category that contains products.";

            _mockCategoryService.Setup(s => s.DeleteCategoryAsync(categoryId))
                .ThrowsAsync(new InvalidOperationException(conflictMessage));

            // Act
            var result = await _controller.DeleteCategory(categoryId);

             
            var conflictResult = result.Should().BeOfType<ConflictObjectResult>().Subject;
            conflictResult.Value.Should().Be(conflictMessage);
        }

        [Fact]
        public async Task DeleteCategory_WhenServiceThrowsException_ReturnsInternalServerError()
        {
             
            var categoryId = 1;
            var exceptionMessage = "Database error";

            _mockCategoryService.Setup(s => s.DeleteCategoryAsync(categoryId))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.DeleteCategory(categoryId);

             
            var statusCodeResult = result.Should().BeOfType<ObjectResult>().Subject;
            statusCodeResult.StatusCode.Should().Be(500);
            statusCodeResult.Value.Should().Be($"Internal server error: {exceptionMessage}");
        }

        #endregion

        #region Constructor Tests

        [Fact]
        public void CategoryController_Constructor_WithValidService_CreatesInstance()
        {
             
            var mockService = new Mock<ICategoryService>();

            // Act
            var controller = new CategoryController(mockService.Object);

             
            controller.Should().NotBeNull();
        }

        #endregion
    }
}