using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;
using FluentAssertions;
using APITask.Controllers;
using APITask.Models.DTOs;
using APITask.Services;
using APITask.Tests.Helpers;

namespace APITask.Tests.Controllers
{
    public class CategoryController_AdvancedTests
    {
        private readonly Mock<ICategoryService> _mockCategoryService;
        private readonly CategoryController _controller;

        public CategoryController_AdvancedTests()
        {
            _mockCategoryService = new Mock<ICategoryService>();
            _controller = new CategoryController(_mockCategoryService.Object);
            SetupControllerContext();
        }

        private void SetupControllerContext()
        {
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        [Theory]
        [InlineData("Fiction", 1)]
        [InlineData("Science", 2)]
        [InlineData("Technology", 3)]
        public async Task CreateCategory_WithSpecificCategoryData_ReturnsCorrectResult(string categoryName, int categoryOrder)
        {
             
            var createDto = TestDataHelper.CreateValidCategoryCreateDTO(categoryName, categoryOrder);
            var expectedResult = TestDataHelper.CreateValidCategoryReadDTO(1, categoryName);

            _mockCategoryService.Setup(s => s.CreateCategoryAsync(It.Is<CategoryCreateDTO>(c => 
                c.CatName == categoryName && c.CatOrder == categoryOrder)))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.CreateCategory(createDto);

             
            var createdAtActionResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
            var returnedCategory = createdAtActionResult.Value.Should().BeOfType<CategoryReadDTO>().Subject;
            
            returnedCategory.CatName.Should().Be(categoryName);
            returnedCategory.Id.Should().Be(expectedResult.Id);
            
            _mockCategoryService.Verify(s => s.CreateCategoryAsync(It.Is<CategoryCreateDTO>(c => 
                c.CatName == categoryName && c.CatOrder == categoryOrder)), Times.Once);
        }

        [Fact]
        public async Task GetCategories_VerifyServiceCallParameters_CallsWithCorrectParameters()
        {
             
            var page = 2;
            var pageSize = 10;
            var categories = TestDataHelper.CreateCategoryList(5);

            _mockCategoryService.Setup(s => s.GetAllCategoriesAsync(page, pageSize))
                .ReturnsAsync(categories);
            _mockCategoryService.Setup(s => s.GetTotalCountAsync())
                .ReturnsAsync(50);

            // Act
            await _controller.GetCategories(page, pageSize);

             
            _mockCategoryService.Verify(s => s.GetAllCategoriesAsync(
                It.Is<int>(p => p == page),
                It.Is<int>(ps => ps == pageSize)), Times.Once);
            
            _mockCategoryService.Verify(s => s.GetTotalCountAsync(), Times.Once);
            _mockCategoryService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateCategory_WithComplexValidation_HandlesAllScenarios()
        {
             
            var categoryId = 5;
            var updateDto = TestDataHelper.CreateValidCategoryUpdateDTO("Updated Category", 99);
            var updatedCategory = TestDataHelper.CreateValidCategoryReadDTO(categoryId, "Updated Category");

            _mockCategoryService.Setup(s => s.UpdateCategoryAsync(categoryId, It.IsAny<CategoryUpdateDTO>()))
                .ReturnsAsync(updatedCategory);

            // Act
            var result = await _controller.UpdateCategory(categoryId, updateDto);

             
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedCategory = okResult.Value.Should().BeOfType<CategoryReadDTO>().Subject;
            
            returnedCategory.Id.Should().Be(categoryId);
            returnedCategory.CatName.Should().Be("Updated Category");

            _mockCategoryService.Verify(s => s.UpdateCategoryAsync(
                It.Is<int>(id => id == categoryId),
                It.Is<CategoryUpdateDTO>(dto => dto.CatName == updateDto.CatName)), Times.Once);
        }

        [Fact]
        public async Task CategoryController_FullWorkflow_SimulatesCompleteLifecycle()
        {
            // This test simulates a complete category lifecycle: Create -> Read -> Update -> Delete

             
            var createDto = TestDataHelper.CreateValidCategoryCreateDTO("Test Category", 1);
            var createdCategory = TestDataHelper.CreateValidCategoryReadDTO(1, "Test Category");
            var updateDto = TestDataHelper.CreateValidCategoryUpdateDTO("Updated Test Category", 2);
            var updatedCategory = TestDataHelper.CreateValidCategoryReadDTO(1, "Updated Test Category");

            // Setup mocks for the full workflow
            _mockCategoryService.Setup(s => s.CreateCategoryAsync(It.IsAny<CategoryCreateDTO>()))
                .ReturnsAsync(createdCategory);
            _mockCategoryService.Setup(s => s.GetCategoryByIdAsync(1))
                .ReturnsAsync(createdCategory);
            _mockCategoryService.Setup(s => s.UpdateCategoryAsync(1, It.IsAny<CategoryUpdateDTO>()))
                .ReturnsAsync(updatedCategory);
            _mockCategoryService.Setup(s => s.DeleteCategoryAsync(1))
                .ReturnsAsync(true);

            // Create
            var createResult = await _controller.CreateCategory(createDto);
            createResult.Result.Should().BeOfType<CreatedAtActionResult>();

            // Read
            var readResult = await _controller.GetCategory(1);
            readResult.Result.Should().BeOfType<OkObjectResult>();

            // Update
            var updateResult = await _controller.UpdateCategory(1, updateDto);
            updateResult.Result.Should().BeOfType<OkObjectResult>();

            // Delete
            var deleteResult = await _controller.DeleteCategory(1);
            deleteResult.Should().BeOfType<NoContentResult>();

            // Verify all service calls were made
            _mockCategoryService.Verify(s => s.CreateCategoryAsync(It.IsAny<CategoryCreateDTO>()), Times.Once);
            _mockCategoryService.Verify(s => s.GetCategoryByIdAsync(1), Times.Once);
            _mockCategoryService.Verify(s => s.UpdateCategoryAsync(1, It.IsAny<CategoryUpdateDTO>()), Times.Once);
            _mockCategoryService.Verify(s => s.DeleteCategoryAsync(1), Times.Once);
        }
    }
}