namespace FastAPI.Domain.Entities;

/// <summary>
/// Stocke les tokens d'authentification.
/// </summary>
public class AuthToken
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; }
    
    public bool IsRevoked { get; set; } = false;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}
