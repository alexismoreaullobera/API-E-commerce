namespace EcommerceAPI.Application.Mappings;

using AutoMapper;
using EcommerceAPI.Application.DTOs;
using EcommerceAPI.Domain.Entities;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();
    }
}