using Microsoft.AspNetCore.Mvc;

namespace FastAPI.API.Controllers;

[ApiController]
[Route("[controller]")]
/// <summary>
/// Contrôleur de vérification de l'état de santé de l'API.
/// </summary>
public class HealthController : ControllerBase
{
    /// <summary>
    /// Vérifie si l'API est en ligne.
    /// </summary>
    /// <returns>Un statut "UP" et l'heure actuelle.</returns>
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { status = "UP", timestamp = DateTime.UtcNow });
    }
}
