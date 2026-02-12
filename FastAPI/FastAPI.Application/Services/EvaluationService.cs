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
    private readonly IStudentRepository _studentRepository;

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="EvaluationService"/>.
    /// </summary>
    /// <param name="evaluationRepository">Le dépôt pour les évaluations.</param>
    /// <param name="studentRepository">Le dépôt pour les étudiants.</param>
    public EvaluationService(IEvaluationRepository evaluationRepository, IStudentRepository studentRepository)
    {
        _evaluationRepository = evaluationRepository;
        _studentRepository = studentRepository;
    }

    /// <summary>
    /// Ajoute une nouvelle évaluation à un étudiant.
    /// </summary>
    /// <param name="dto">Les données de création de l'évaluation.</param>
    /// <returns>Le DTO de l'évaluation créée.</returns>
    /// <exception cref="Exception">Levée si l'étudiant n'est pas trouvé.</exception>
    public async Task<EvaluationDto> AddEvaluationAsync(CreateEvaluationDto dto)
    {
        var student = await _studentRepository.GetByIdAsync(dto.StudentId);
        if (student == null) throw new Exception("Student not found");

        var evaluation = new Evaluation
        {
            Name = dto.Name,
            Description = dto.Description,
            StudentId = dto.StudentId,
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
            evaluation.Criteria.Select(c => new CriterionDto(c.Id, c.Name, c.Description, c.CompetenceType)).ToList()
        );
    }
}
