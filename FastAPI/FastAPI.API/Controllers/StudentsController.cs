using FastAPI.Application.DTOs;
using FastAPI.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FastAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
/// <summary>
/// Contrôleur gérant les opérations sur les étudiants.
/// </summary>
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="StudentsController"/>.
    /// </summary>
    /// <param name="studentService">Le service de gestion des étudiants.</param>
    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    /// <summary>
    /// Récupère la liste de tous les étudiants.
    /// </summary>
    /// <returns>Une liste de DTOs d'étudiants.</returns>
    [HttpGet]
    public async Task<ActionResult<List<StudentDto>>> GetAll()
    {
        return Ok(await _studentService.GetAllAsync());
    }

    /// <summary>
    /// Récupère un étudiant spécifique par son ID.
    /// </summary>
    /// <param name="id">L'identifiant de l'étudiant.</param>
    /// <returns>Le DTO de l'étudiant si trouvé, sinon 404 NotFound.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<StudentDto>> GetById(Guid id)
    {
        var student = await _studentService.GetByIdAsync(id);
        if (student == null) return NotFound();
        return Ok(student);
    }

    /// <summary>
    /// Crée un nouvel étudiant.
    /// </summary>
    /// <param name="dto">Les données de création de l'étudiant.</param>
    /// <returns>Le DTO de l'étudiant créé avec le code 201 Created.</returns>
    [HttpPost]
    public async Task<ActionResult<StudentDto>> Create(CreateStudentDto dto)
    {
        var student = await _studentService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
    }
}
