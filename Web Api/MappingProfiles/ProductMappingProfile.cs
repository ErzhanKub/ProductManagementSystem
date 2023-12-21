using AutoMapper;
using Web_Api.Models.Contracts.ProductDto;
using Web_Api.Models.Entities;

namespace Web_Api.MappingProfiles;

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
