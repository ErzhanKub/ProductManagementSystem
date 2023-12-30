using AutoMapper;
using WebApi.Models.Contracts.CategoryDto;
using WebApi.Models.Entities;

namespace WebApi.MappingProfiles;

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
