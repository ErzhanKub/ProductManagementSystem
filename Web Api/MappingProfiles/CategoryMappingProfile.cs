using AutoMapper;
using Web_Api.Models.Contracts.CategoryDto;
using Web_Api.Models.Entities;

namespace Web_Api.MappingProfiles;

internal sealed class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<Category, CategoryGetDto>();
        CreateMap<CategoryGetDto, Category>();
        CreateMap<CategoryPostDto, Category>();
        CreateMap<Category, CategoryPostDto>();
    }
}
