using EcommerceAPI.Application.DTOs;
using EcommerceAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Permet l'enregistrement d'un nouvel utilisateur. Si l'enregistrement est réussi, un token d'authentification est retourné. En cas d'erreur (par exemple, si l'email est déjà utilisé), un message d'erreur est renvoyé.
    /// </summary>
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        try
        {
            AuthResponseDto responseDto = await _authService.RegisterAsync(dto);
            return Ok(responseDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Permet à un utilisateur existant de se connecter en fournissant ses identifiants. Si les identifiants sont valides, un token d'authentification est retourné. En cas d'échec (par exemple, si les identifiants sont incorrects), une réponse "Unauthorized" est renvoyée avec un message d'erreur.
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        AuthResponseDto? responseDto = await _authService.LoginAsync(dto);
        if (responseDto == null)
        {
            return Unauthorized(new { message = "Invalid credentials." });
        }
        return Ok(responseDto);
    }
}
