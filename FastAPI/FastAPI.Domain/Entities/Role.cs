namespace FastAPI.Domain.Entities;

/// <summary>
/// Représente un rôle (ex: Étudiant, Enseignant).
/// </summary>
public class Role
{
    /// <summary>
    /// Identifiant unique du rôle.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Nom du rôle (ex: "Student", "Teacher").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description des accès ou permissions.
    /// </summary>
    public string Access { get; set; } = string.Empty;
}
