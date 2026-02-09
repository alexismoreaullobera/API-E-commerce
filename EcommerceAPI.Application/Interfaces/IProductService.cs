using EcommerceAPI.Application.DTOs;

namespace EcommerceAPI.Application.Interfaces;

public interface IProductService
{
    Task<ProductDto> CreateProductAsync(CreateProductDto createDto);
    Task<ProductDto> UpdateProductAsync(Guid id, UpdateProductDto updateDto);
    Task<ProductDto?> GetProductByIdAsync(Guid id);
    Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    Task DeleteProductAsync(Guid id);
}