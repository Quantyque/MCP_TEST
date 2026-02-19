using System.Security.Claims;
using System.Text.Encodings.Web;
using FastAPI.Application.Interfaces;
using FastAPI.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FastAPI.API.Auth;

public class TokenAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
}

public class TokenAuthenticationHandler : AuthenticationHandler<TokenAuthenticationSchemeOptions>
{
    private readonly ApplicationDbContext _context;

    public TokenAuthenticationHandler(
        IOptionsMonitor<TokenAuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        ApplicationDbContext context)
        : base(options, logger, encoder, clock)
    {
        _context = context;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
            return AuthenticateResult.Fail("Missing Authorization Header");

        var authHeader = Request.Headers["Authorization"].ToString();
        if (!authHeader.StartsWith("Bearer "))
            return AuthenticateResult.Fail("Invalid Authorization Header");

        var token = authHeader.Substring("Bearer ".Length).Trim();

        var authToken = await _context.AuthTokens
            .Include(t => t.User)
            .ThenInclude(u => u.BuildingRoles)
            .ThenInclude(br => br.Role)
            .FirstOrDefaultAsync(t => t.AccessToken == token && !t.IsRevoked && t.ExpiryDate > DateTime.UtcNow);

        if (authToken == null)
            return AuthenticateResult.Fail("Invalid or Expired Token");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, authToken.User.Id.ToString()),
            new Claim(ClaimTypes.Name, $"{authToken.User.FirstName} {authToken.User.LastName}"),
            new Claim(ClaimTypes.Email, authToken.User.Email)
        };

        // Add roles as claims (format: "RoleName:BuildingId")
        foreach (var role in authToken.User.BuildingRoles)
        {
            claims.Add(new Claim("BuildingRole", $"{role.Role.Name}:{role.BuildingId}"));
        }

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}
