namespace FastAPI.Domain.Entities;

/// <summary>
/// Représente un étudiant dans le système.
/// </summary>
public class Student
{
    /// <summary>
    /// Identifiant unique de l'étudiant.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Prénom de l'étudiant.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Nom de famille de l'étudiant.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Adresse de l'étudiant.
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Liste des évaluations associées à cet étudiant.
    /// </summary>
    public ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();
}
