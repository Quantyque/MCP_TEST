using FastAPI.Application.DTOs;
using FastAPI.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FastAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EvaluationsController : ControllerBase
{
    private readonly IEvaluationService _evaluationService;

    public EvaluationsController(IEvaluationService evaluationService)
    {
        _evaluationService = evaluationService;
    }

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
