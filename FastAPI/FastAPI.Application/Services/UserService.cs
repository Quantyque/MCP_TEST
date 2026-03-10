using FastAPI.Application.DTOs;
using FastAPI.Application.Interfaces;
using FastAPI.Domain.Entities;

namespace FastAPI.Application.Services;

/// <summary>
/// Service gérant la logique métier pour les utilisateurs.
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Récupère la liste de tous les utilisateurs.
    /// </summary>
    /// <returns>Une liste de DTOs d'utilisateurs.</returns>
    public async Task<List<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(MapToDto).ToList();
    }

    /// <summary>
    /// Récupère un utilisateur par son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant de l'utilisateur.</param>
    /// <returns>Le DTO de l'utilisateur si trouvé, sinon null.</returns>
    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user == null ? null : MapToDto(user);
    }

    /// <summary>
    /// Récupère un utilisateur par son email.
    /// </summary>
    /// <param name="email">L'email de l'utilisateur.</param>
    /// <returns>Le DTO de l'utilisateur si trouvé, sinon null.</returns>
    public async Task<UserDto?> GetByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        return user == null ? null : MapToDto(user);
    }

    /// <summary>
    /// Crée un nouvel utilisateur.
    /// </summary>
    /// <param name="dto">Les données pour créer l'utilisateur.</param>
    /// <returns>Le DTO de l'utilisateur créé.</returns>
    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        // TODO: Hash password
        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PasswordHash = dto.Password // Insecure for now, as requested simple auth
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return MapToDto(user);
    }

    private static UserDto MapToDto(User u) => new(
        u.Id,
        u.FirstName,
        u.LastName,
        u.Email,
        u.BuildingRoles.Select(br => $"{br.Role.Name} at {br.Building.Name}").ToList()
    );
}
