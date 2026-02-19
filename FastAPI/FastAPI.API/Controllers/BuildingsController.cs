using FastAPI.Application.DTOs;
using FastAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FastAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
/// <summary>
/// Contrôleur gérant les opérations sur les bâtiments.
/// </summary>
public class BuildingsController : ControllerBase
{
    private readonly IBuildingService _buildingService;

    public BuildingsController(IBuildingService buildingService)
    {
        _buildingService = buildingService;
    }

    /// <summary>
    /// Récupère la liste de tous les bâtiments.
    /// </summary>
    /// <returns>Une liste de DTOs de bâtiments.</returns>
    [HttpGet]
    public async Task<ActionResult<List<BuildingDto>>> GetAll()
    {
        return Ok(await _buildingService.GetAllAsync());
    }

    /// <summary>
    /// Récupère un bâtiment spécifique par son ID.
    /// </summary>
    /// <param name="id">L'identifiant du bâtiment.</param>
    /// <returns>Le DTO du bâtiment si trouvé, sinon 404 NotFound.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<BuildingDto>> GetById(Guid id)
    {
        var building = await _buildingService.GetByIdAsync(id);
        if (building == null) return NotFound();
        return Ok(building);
    }

    /// <summary>
    /// Crée un nouveau bâtiment.
    /// </summary>
    /// <param name="dto">Les données de création du bâtiment.</param>
    /// <returns>Le DTO du bâtiment créé avec le code 201 Created.</returns>
    [HttpPost]
    public async Task<ActionResult<BuildingDto>> Create(CreateBuildingDto dto)
    {
        var building = await _buildingService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = building.Id }, building);
    }
}
