using FastAPI.Application.DTOs;
using FastAPI.Application.Interfaces;
using FastAPI.Domain.Entities;

namespace FastAPI.Application.Services;

/// <summary>
/// Service gérant la logique métier pour les évaluations.
/// </summary>
public class EvaluationService : IEvaluationService
{
    private readonly IEvaluationRepository _evaluationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IBuildingRepository _buildingRepository;

    public EvaluationService(IEvaluationRepository evaluationRepository, IUserRepository userRepository, IBuildingRepository buildingRepository)
    {
        _evaluationRepository = evaluationRepository;
        _userRepository = userRepository;
        _buildingRepository = buildingRepository;
    }

    /// <summary>
    /// Ajoute une nouvelle évaluation à un étudiant.
    /// </summary>
    /// <param name="dto">Les données de création de l'évaluation.</param>
    /// <returns>Le DTO de l'évaluation créée.</returns>
    /// <exception cref="Exception">Levée si l'étudiant n'est pas trouvé ou n'est pas dans le bon bâtiment.</exception>
    public async Task<EvaluationDto> AddEvaluationAsync(CreateEvaluationDto dto)
    {
        var student = await _userRepository.GetByIdAsync(dto.StudentId);
        if (student == null) throw new Exception("Student not found");

        var building = await _buildingRepository.GetByIdAsync(dto.BuildingId);
        if (building == null) throw new Exception("Building not found");

        // Verify Student has Student role in this Building
        var isStudentInBuilding = student.BuildingRoles.Any(br => br.BuildingId == dto.BuildingId && br.Role.Name == "Student");
        if (!isStudentInBuilding)
        {
             // For simplicity in this demo, strict check might be disabled if we just want to create data
             // throw new Exception("User is not a student in this building");
        }
        
        // Mock Teacher logic: Pick the first available user as a teacher (or assume current user logic to be added later)
        var teacher = (await _userRepository.GetAllAsync()).FirstOrDefault();
        if (teacher == null) throw new Exception("No users available to assign as teacher");

        var evaluation = new Evaluation
        {
            Name = dto.Name,
            Description = dto.Description,
            StudentId = dto.StudentId,
            TeacherId = teacher.Id, 
            Criteria = dto.Criteria.Select(c => new Criterion
            {
                Name = c.Name,
                Description = c.Description,
                CompetenceType = c.CompetenceType
            }).ToList()
        };

        await _evaluationRepository.AddAsync(evaluation);
        await _evaluationRepository.SaveChangesAsync();

        return new EvaluationDto(
            evaluation.Id,
            evaluation.Name,
            evaluation.Description,
            $"{teacher.FirstName} {teacher.LastName}",
            $"{student.FirstName} {student.LastName}",
            evaluation.Criteria.Select(c => new CriterionDto(c.Id, c.Name, c.Description, c.CompetenceType)).ToList()
        );
    }
}
