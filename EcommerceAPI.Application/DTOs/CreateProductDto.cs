namespace EcommerceAPI.Application.DTOs;

public record CreateProductDto(
    string Name,
    string Description,
    decimal Price,
    int Stock,
    string ImageUrl
);