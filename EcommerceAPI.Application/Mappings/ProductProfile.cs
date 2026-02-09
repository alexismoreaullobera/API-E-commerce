using AutoMapper;
using EcommerceAPI.Application.DTOs;
using EcommerceAPI.Domain.Entities;

namespace EcommerceAPI.Application.Mappings;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();
    }
}