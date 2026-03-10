using FastAPI.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FastAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
/// <summary>
/// Contrôleur gérant la récupération des compétences disponibles.
/// </summary>
public class CompetenciesController : ControllerBase
{
    /// <summary>
    /// Récupère la liste de toutes les compétences (types de compétences).
    /// </summary>
    /// <returns>Une liste d'objets contenant l'ID et le nom de la compétence.</returns>
    [HttpGet]
    public IActionResult GetAll()
    {
        var competencies = Enum.GetValues<CompetenceType>()
            .Select(c => new
            {
                Id = (int)c,
                Name = c.ToString()
            });
        
        return Ok(competencies);
    }
}
