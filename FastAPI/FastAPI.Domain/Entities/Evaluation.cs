namespace FastAPI.Domain.Entities;

/// <summary>
/// Représente une évaluation d'un étudiant.
/// </summary>
public class Evaluation
{
    /// <summary>
    /// Identifiant unique de l'évaluation.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Nom de l'évaluation.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description de l'évaluation.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    // Foreign Key
    /// <summary>
    /// Identifiant de l'étudiant associé.
    /// </summary>
    public Guid StudentId { get; set; }

    /// <summary>
    /// L'étudiant associé à cette évaluation.
    /// </summary>
    public Student Student { get; set; } = null!;

    /// <summary>
    /// Liste des critères évalués.
    /// </summary>
    public ICollection<Criterion> Criteria { get; set; } = new List<Criterion>();
}
