using FastAPI.Application.DTOs;
using FastAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FastAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
/// <summary>
/// Contrôleur gérant les opérations sur les utilisateurs.
/// </summary>
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Récupère la liste de tous les utilisateurs.
    /// </summary>
    /// <returns>Une liste de DTOs d'utilisateurs.</returns>
    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetAll()
    {
        return Ok(await _userService.GetAllAsync());
    }

    /// <summary>
    /// Récupère un utilisateur spécifique par son ID.
    /// </summary>
    /// <param name="id">L'identifiant de l'utilisateur.</param>
    /// <returns>Le DTO de l'utilisateur si trouvé, sinon 404 NotFound.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetById(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    /// <summary>
    /// Crée un nouvel utilisateur.
    /// </summary>
    /// <param name="dto">Les données de création de l'utilisateur.</param>
    /// <returns>Le DTO de l'utilisateur créé avec le code 201 Created.</returns>
    [HttpPost]
    public async Task<ActionResult<UserDto>> Create(CreateUserDto dto)
    {
        var user = await _userService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }
}
