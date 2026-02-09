using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using EcommerceAPI.Application.DTOs;
using EcommerceAPI.Application.Interfaces;
using EcommerceAPI.Domain.Entities;
using EcommerceAPI.Domain.Interfaces;

namespace EcommerceAPI.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductDto createDto)
    {
        Product productToCreate = _mapper.Map<Product>(createDto);

        Product createdProduct = await _repository.CreateAsync(productToCreate);

        return _mapper.Map<ProductDto>(createdProduct);
    }

    public async Task DeleteProductAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        // IEnumerable<Product> products = await _repository.GetAllAsync();
        // List<ProductDto> productDtos = new List<ProductDto>();

        // foreach (Product product in products)
        // {
        //     productDtos.Add(_mapper.Map<ProductDto>(product));
        // }

        return (await _repository.GetAllAsync()).Select(x => _mapper.Map<ProductDto>(x));
    }

    public async Task<ProductDto> GetProductByIdAsync(Guid id)
    {
        return _mapper.Map<ProductDto>(await _repository.GetByIdAsync(id));
    }

    public async Task<ProductDto> UpdateProductAsync(Guid id, UpdateProductDto updateDto)
    {
        Product productToUpdate = _mapper.Map<Product>(updateDto);

        Product updatedProduct = await _repository.UpdateAsync(productToUpdate);

        return _mapper.Map<ProductDto>(updatedProduct);
    }
}