namespace FastAPI.Domain.Entities;

/// <summary>
/// Table de liaison définissant le rôle d'un utilisateur dans un bâtiment spécifique.
/// </summary>
public class UserBuildingRole
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid BuildingId { get; set; }
    public Building Building { get; set; } = null!;

    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;
}
