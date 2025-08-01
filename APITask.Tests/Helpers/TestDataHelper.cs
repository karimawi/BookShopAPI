using AutoFixture;
using APITask.Models.DTOs;

namespace APITask.Tests.Helpers
{
    public static class TestDataHelper
    {
        private static readonly IFixture _fixture = new Fixture();

        public static CategoryReadDTO CreateValidCategoryReadDTO(int? id = null, string? name = null)
        {
            var category = _fixture.Create<CategoryReadDTO>();
            if (id.HasValue) category.Id = id.Value;
            if (!string.IsNullOrEmpty(name)) category.CatName = name;
            return category;
        }

        public static CategoryCreateDTO CreateValidCategoryCreateDTO(string? name = null, int? order = null)
        {
            var category = _fixture.Create<CategoryCreateDTO>();
            if (!string.IsNullOrEmpty(name)) category.CatName = name;
            if (order.HasValue) category.CatOrder = order.Value;
            return category;
        }

        public static CategoryUpdateDTO CreateValidCategoryUpdateDTO(string? name = null, int? order = null)
        {
            var category = _fixture.Create<CategoryUpdateDTO>();
            if (!string.IsNullOrEmpty(name)) category.CatName = name;
            if (order.HasValue) category.CatOrder = order.Value;
            return category;
        }

        public static List<CategoryReadDTO> CreateCategoryList(int count = 3)
        {
            return _fixture.CreateMany<CategoryReadDTO>(count).ToList();
        }
    }
}