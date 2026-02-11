using EcommerceAPI.Application.Interfaces;
using EcommerceAPI.Application.DTOs;
using EcommerceAPI.Domain.Entities;
using AutoMapper;
using EcommerceAPI.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EcommerceAPI.Application.Services;

public class AuthService : IAuthService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _repository;
    private readonly IConfiguration _configuration;

    public AuthService(IMapper mapper, IUserRepository userRepository, IConfiguration configuration)
    {
        _mapper = mapper;
        _repository = userRepository;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        User? existingUser = await _repository.GetByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            throw new Exception("Email already in use");
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
        string? secretKey = _configuration["Jwt:SecretKey"];
        string? issuer = _configuration["Jwt:Issuer"];
        string? audience = _configuration["Jwt:Audience"];
        int expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"]!);

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        Claim[] claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
