using AutoMapper;
using WebApi.Models.Contracts.ProductDto;
using WebApi.Models.Entities;

namespace WebApi.MappingProfiles;

internal sealed class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Product, ProductGetDto>();
        CreateMap<ProductGetDto, Product>();
        CreateMap<ProductPostDto, Product>();
        CreateMap<Product, ProductPostDto>();
    }
}
