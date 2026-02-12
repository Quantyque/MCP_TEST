using FastAPI.Domain.Entities;

namespace FastAPI.Application.Interfaces;

/// <summary>
/// Interface pour le dépôt des étudiants.
/// </summary>
public interface IStudentRepository
{
    /// <summary>
    /// Récupère tous les étudiants.
    /// </summary>
    Task<List<Student>> GetAllAsync();

    /// <summary>
    /// Récupère un étudiant par son ID.
    /// </summary>
    Task<Student?> GetByIdAsync(Guid id);

    /// <summary>
    /// Ajoute un étudiant.
    /// </summary>
    Task AddAsync(Student student);

    /// <summary>
    /// Sauvegarde les changements.
    /// </summary>
    Task SaveChangesAsync();
}

/// <summary>
/// Interface pour le dépôt des évaluations.
/// </summary>
public interface IEvaluationRepository
{
    /// <summary>
    /// Ajoute une évaluation.
    /// </summary>
    Task AddAsync(Evaluation evaluation);

    /// <summary>
    /// Sauvegarde les changements.
    /// </summary>
    Task SaveChangesAsync();
}
