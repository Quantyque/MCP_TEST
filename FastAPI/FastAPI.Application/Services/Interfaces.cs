using FastAPI.Application.DTOs;

namespace FastAPI.Application.Services;

/// <summary>
/// Interface pour le service de gestion des étudiants.
/// </summary>
public interface IStudentService
{
    /// <summary>
    /// Récupère tous les étudiants.
    /// </summary>
    Task<List<StudentDto>> GetAllAsync();

    /// <summary>
    /// Récupère un étudiant par son ID.
    /// </summary>
    Task<StudentDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Crée un nouvel étudiant.
    /// </summary>
    Task<StudentDto> CreateAsync(CreateStudentDto dto);
}

/// <summary>
/// Interface pour le service de gestion des évaluations.
/// </summary>
public interface IEvaluationService
{
    /// <summary>
    /// Ajoute une évaluation.
    /// </summary>
    Task<EvaluationDto> AddEvaluationAsync(CreateEvaluationDto dto);
}
