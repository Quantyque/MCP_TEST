using FastAPI.Application.DTOs;
using FastAPI.Application.Interfaces;
using FastAPI.Domain.Entities;

namespace FastAPI.Application.Services;

/// <summary>
/// Service gérant la logique métier pour les bâtiments.
/// </summary>
public class BuildingService : IBuildingService
{
    private readonly IBuildingRepository _buildingRepository;

    public BuildingService(IBuildingRepository buildingRepository)
    {
        _buildingRepository = buildingRepository;
    }

    /// <summary>
    /// Récupère la liste de tous les bâtiments.
    /// </summary>
    /// <returns>Une liste de DTOs représentant les bâtiments.</returns>
    public async Task<List<BuildingDto>> GetAllAsync()
    {
        var buildings = await _buildingRepository.GetAllAsync();
        return buildings.Select(b => new BuildingDto(b.Id, b.Name, b.Address)).ToList();
    }

    /// <summary>
    /// Récupère un bâtiment par son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant du bâtiment.</param>
    /// <returns>Le DTO du bâtiment si trouvé, sinon null.</returns>
    public async Task<BuildingDto?> GetByIdAsync(Guid id)
    {
        var building = await _buildingRepository.GetByIdAsync(id);
        return building == null ? null : new BuildingDto(building.Id, building.Name, building.Address);
    }

    /// <summary>
    /// Crée un nouveau bâtiment.
    /// </summary>
    /// <param name="dto">Les données pour créer le bâtiment.</param>
    /// <returns>Le DTO du bâtiment créé.</returns>
    public async Task<BuildingDto> CreateAsync(CreateBuildingDto dto)
    {
        var building = new Building
        {
            Name = dto.Name,
            Address = dto.Address
        };

        await _buildingRepository.AddAsync(building);
        await _buildingRepository.SaveChangesAsync();

        return new BuildingDto(building.Id, building.Name, building.Address);
    }
}
