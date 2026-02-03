using EcommerceAPI.Domain.Entities;
using EcommerceAPI.Domain.Interfaces;
using EcommerceAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPi.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Product> CreateAsync(Product product)
    {
        product.Id = Guid.NewGuid();
        product.CreatedAt = DateTime.Now;
        product.UpdatedAt = DateTime.Now;

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return product;
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Product>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Product?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Product> UpdateAsync(Product product)
    {
        throw new NotImplementedException();
    }
}