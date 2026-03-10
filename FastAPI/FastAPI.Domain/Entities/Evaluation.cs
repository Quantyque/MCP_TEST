namespace FastAPI.Domain.Entities;

/// <summary>
/// Représente une évaluation.
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

    /// <summary>
    /// L'étudiant évalué.
    /// </summary>
    public Guid StudentId { get; set; }
    public User Student { get; set; } = null!;

    /// <summary>
    /// L'enseignant qui a créé l'évaluation.
    /// </summary>
    public Guid TeacherId { get; set; }
    public User Teacher { get; set; } = null!;

    /// <summary>
    /// Liste des critères évalués.
    /// </summary>
    public ICollection<Criterion> Criteria { get; set; } = new List<Criterion>();
}
