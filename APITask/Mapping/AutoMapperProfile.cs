using AutoMapper;
using APITask.Models.Entities;
using APITask.Models.DTOs;

namespace APITask.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Category mappings
            CreateMap<Category, CategoryReadDTO>();
            CreateMap<CategoryCreateDTO, Category>();
            CreateMap<CategoryUpdateDTO, Category>();
            CreateMap<Category, CategoryUpdateDTO>();

            // Product mappings
            CreateMap<Product, ProductReadDTO>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => GetCategoryName(src.Category)));
            
            CreateMap<ProductCreateDTO, Product>();
            CreateMap<ProductUpdateDTO, Product>();
            CreateMap<Product, ProductUpdateDTO>();
        }

        private static string GetCategoryName(Category? category)
        {
            return category?.CatName ?? string.Empty;
        }
    }
}