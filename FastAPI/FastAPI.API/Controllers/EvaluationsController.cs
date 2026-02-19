using FastAPI.Application.DTOs;
using FastAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FastAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
/// <summary>
/// Contrôleur pour la gestion des évaluations.
/// </summary>
public class EvaluationsController : ControllerBase
{
    private readonly IEvaluationService _evaluationService;

    public EvaluationsController(IEvaluationService evaluationService)
    {
        _evaluationService = evaluationService;
    }

    /// <summary>
    /// Crée une nouvelle évaluation pour un étudiant.
    /// </summary>
    /// <param name="dto">Les données de l'évaluation à créer.</param>
    /// <returns>Le DTO de l'évaluation créée.</returns>
    [HttpPost]
    public async Task<ActionResult<EvaluationDto>> Create(CreateEvaluationDto dto)
    {
        try
        {
            var evaluation = await _evaluationService.AddEvaluationAsync(dto);
            return Ok(evaluation);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
