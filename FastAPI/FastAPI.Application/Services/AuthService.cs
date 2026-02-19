using FastAPI.Application.DTOs;
using FastAPI.Application.Interfaces;
using FastAPI.Domain.Entities;
using System.Security.Cryptography;

namespace FastAPI.Application.Services;

/// <summary>
/// Service gérant l'authentification et la génération de tokens.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Authentifie un utilisateur et génère les tokens d'accès.
    /// </summary>
    /// <param name="dto">Les informations de connexion.</param>
    /// <returns>Les tokens d'accès et de rafraîchissement.</returns>
    /// <exception cref="Exception">Levée si les identifiants sont invalides.</exception>
    public async Task<TokenDto> LoginAsync(LoginDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null || user.PasswordHash != dto.Password) // Simple check
        {
            throw new Exception("Invalid credentials");
        }

        // Generate Tokens (Mock implementation for "OAuth2 Authorization Code Grant" simulation)
        // Ideally this would involve code exchange, but for login endpoint we return tokens directly
        // to simplify testing here. In real OAuth2, this would be the token endpoint called after code exchange.
        
        var accessToken = GenerateToken();
        var refreshToken = GenerateToken();

        var authToken = new AuthToken
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiryDate = DateTime.UtcNow.AddHours(1),
            UserId = user.Id
        };
        
        user.AuthTokens.Add(authToken);
        await _userRepository.SaveChangesAsync();

        return new TokenDto(accessToken, refreshToken);
    }

    /// <summary>
    /// Génère une chaîne de caractères aléatoire sécurisée pour le token.
    /// </summary>
    /// <returns>Le token généré.</returns>
    private string GenerateToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }
}
