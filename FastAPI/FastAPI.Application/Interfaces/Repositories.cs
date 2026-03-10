using FastAPI.Domain.Entities;

namespace FastAPI.Application.Interfaces;

/// <summary>
/// Interface pour le dépôt des utilisateurs.
/// </summary>
public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task AddAsync(User user);
    Task SaveChangesAsync();
}

/// <summary>
/// Interface pour le dépôt des bâtiments.
/// </summary>
public interface IBuildingRepository
{
    Task<List<Building>> GetAllAsync();
    Task<Building?> GetByIdAsync(Guid id);
    Task AddAsync(Building building);
    Task SaveChangesAsync();
}

/// <summary>
/// Interface pour le dépôt des évaluations.
/// </summary>
public interface IEvaluationRepository
{
    Task AddAsync(Evaluation evaluation);
    Task<List<Evaluation>> GetByStudentIdAsync(Guid studentId);
    Task SaveChangesAsync();
}
