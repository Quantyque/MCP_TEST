using FastAPI.Application.DTOs;

namespace FastAPI.Application.Interfaces;

/// <summary>
/// Interface pour le service de gestion des utilisateurs.
/// </summary>
public interface IUserService
{
    Task<List<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(Guid id);
    Task<UserDto> CreateAsync(CreateUserDto dto);
    Task<UserDto?> GetByEmailAsync(string email);
}

/// <summary>
/// Interface pour le service de gestion des bâtiments.
/// </summary>
public interface IBuildingService
{
    Task<List<BuildingDto>> GetAllAsync();
    Task<BuildingDto?> GetByIdAsync(Guid id);
    Task<BuildingDto> CreateAsync(CreateBuildingDto dto);
}

/// <summary>
/// Interface pour le service de gestion des évaluations.
/// </summary>
public interface IEvaluationService
{
    Task<EvaluationDto> AddEvaluationAsync(CreateEvaluationDto dto);
}

/// <summary>
/// Interface pour le service d'authentification.
/// </summary>
public interface IAuthService
{
    Task<TokenDto> LoginAsync(LoginDto dto);
}
