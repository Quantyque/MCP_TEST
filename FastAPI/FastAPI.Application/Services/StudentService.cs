using FastAPI.Application.DTOs;
using FastAPI.Application.Interfaces;
using FastAPI.Domain.Entities;

namespace FastAPI.Application.Services;

/// <summary>
/// Service gérant la logique métier liée aux étudiants.
/// </summary>
public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="StudentService"/>.
    /// </summary>
    /// <param name="studentRepository">Le dépôt pour l'accès aux données des étudiants.</param>
    public StudentService(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    /// <summary>
    /// Récupère tous les étudiants de manière asynchrone.
    /// </summary>
    /// <returns>Une liste de DTOs d'étudiants.</returns>
    public async Task<List<StudentDto>> GetAllAsync()
    {
        var students = await _studentRepository.GetAllAsync();
        return students.Select(MapToDto).ToList();
    }

    /// <summary>
    /// Récupère un étudiant par son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant de l'étudiant.</param>
    /// <returns>Le DTO de l'étudiant si trouvé, sinon null.</returns>
    public async Task<StudentDto?> GetByIdAsync(Guid id)
    {
        var student = await _studentRepository.GetByIdAsync(id);
        return student == null ? null : MapToDto(student);
    }

    /// <summary>
    /// Crée un nouvel étudiant.
    /// </summary>
    /// <param name="dto">Les données pour créer l'étudiant.</param>
    /// <returns>Le DTO de l'étudiant créé.</returns>
    public async Task<StudentDto> CreateAsync(CreateStudentDto dto)
    {
        var student = new Student
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Address = dto.Address
        };

        await _studentRepository.AddAsync(student);
        await _studentRepository.SaveChangesAsync();

        return MapToDto(student);
    }

    private static StudentDto MapToDto(Student s) => new(
        s.Id,
        s.FirstName,
        s.LastName,
        s.Address,
        s.Evaluations.Select(e => new EvaluationDto(
            e.Id,
            e.Name,
            e.Description,
            e.Criteria.Select(c => new CriterionDto(c.Id, c.Name, c.Description, c.CompetenceType)).ToList()
        )).ToList()
    );
}
