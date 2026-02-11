using EcommerceAPI.Domain.Entities;

namespace EcommerceAPI.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User> CreateAsync(User user);
}
