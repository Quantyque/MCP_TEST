using FastAPI.Domain.Enums;

namespace FastAPI.Domain.Entities;

/// <summary>
/// Représente un critère d'évaluation.
/// </summary>
public class Criterion
{
    /// <summary>
    /// Identifiant unique du critère.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Nom du critère.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description du critère.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Type de compétence évaluée.
    /// </summary>
    public CompetenceType CompetenceType { get; set; }

    // Foreign Key
    /// <summary>
    /// Identifiant de l'évaluation parente.
    /// </summary>
    public Guid EvaluationId { get; set; }

    /// <summary>
    /// L'évaluation à laquelle ce critère appartient.
    /// </summary>
    public Evaluation Evaluation { get; set; } = null!;
}
