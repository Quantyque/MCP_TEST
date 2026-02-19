namespace FastAPI.Domain.Entities;

/// <summary>
/// Représente un bâtiment ou un campus.
/// </summary>
public class Building
{
    /// <summary>
    /// Identifiant unique du bâtiment.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Nom du bâtiment.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Adresse du bâtiment.
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Liste des rôles utilisateurs associés à ce bâtiment.
    /// </summary>
    public ICollection<UserBuildingRole> UserRoles { get; set; } = new List<UserBuildingRole>();
}
