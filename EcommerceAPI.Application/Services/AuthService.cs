using EcommerceAPI.Application.Interfaces;
using EcommerceAPI.Application.DTOs;
using EcommerceAPI.Domain.Entities;
using AutoMapper;
using EcommerceAPI.Domain.Interfaces;

namespace EcommerceAPI.Application.Services;

public class AuthService : IAuthService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _repository;

    public AuthService(IMapper mapper, IUserRepository userRepository)
    {
        _mapper = mapper;
        _repository = userRepository;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        User existingUser = _mapper.Map<User>(dto);
        if (existingUser == null)
        {
            throw new Exception("User already exists");
        }

        User user = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = "User",
            CreatedAt = DateTime.UtcNow
        };

        await _repository.CreateAsync(user);

        var token = GenerateJwtToken(user);

        return new AuthResponseDto(token, user.Email, user.Role);
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        User? user = await _repository.GetByEmailAsync(dto.Email);
        if (user == null)
        {
            return null;
        }

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            return null;
        }

        user.LastLoginAt = DateTime.UtcNow;
        //TODO: MAJ du USer

        var token = GenerateJwtToken(user);
        return new AuthResponseDto(token, user.Email, user.Role);
    }

    private string GenerateJwtToken(User user)
    {
        // TODO: À implémenter avec System.IdentityModel.Tokens.Jwt
        throw new NotImplementedException();
    }
}
