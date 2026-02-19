using FastAPI.Application.DTOs;
using FastAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FastAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
/// <summary>
/// Contrôleur pour l'authentification.
/// </summary>
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Authentifie un utilisateur et retourne les tokens.
    /// </summary>
    /// <param name="dto">Les identifiants de connexion.</param>
    /// <returns>Les tokens d'accès et de rafraîchissement.</returns>
    [HttpPost("login")]
    public async Task<ActionResult<TokenDto>> Login(LoginDto dto)
    {
        try
        {
            var tokens = await _authService.LoginAsync(dto);
            return Ok(tokens);
        }
        catch (Exception ex)
        {
            return Unauthorized(ex.Message);
        }
    }
}
