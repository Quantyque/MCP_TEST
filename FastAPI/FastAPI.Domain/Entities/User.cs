namespace FastAPI.Domain.Entities;

/// <summary>
/// Représente un utilisateur du système.
/// </summary>
public class User
{
    /// <summary>
    /// Identifiant unique de l'utilisateur.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Prénom de l'utilisateur.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Nom de famille de l'utilisateur.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Adresse email de l'utilisateur.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Mot de passe hashé (pour simplifier, on stockera peut-être en clair ou hash basique pour ce test).
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Numéro de téléphone.
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Rôles de l'utilisateur dans différents bâtiments.
    /// </summary>
    public ICollection<UserBuildingRole> BuildingRoles { get; set; } = new List<UserBuildingRole>();

    /// <summary>
    /// Tokens d'authentification associés.
    /// </summary>
    public ICollection<AuthToken> AuthTokens { get; set; } = new List<AuthToken>();

    /// <summary>
    /// Évaluations reçues (en tant qu'étudiant).
    /// </summary>
    public ICollection<Evaluation> ReceivedEvaluations { get; set; } = new List<Evaluation>();

    /// <summary>
    /// Évaluations données/créées (en tant qu'enseignant).
    /// </summary>
    public ICollection<Evaluation> GivenEvaluations { get; set; } = new List<Evaluation>();
}
