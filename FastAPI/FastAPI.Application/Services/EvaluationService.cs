using FastAPI.Application.DTOs;
using FastAPI.Application.Interfaces;
using FastAPI.Domain.Entities;

namespace FastAPI.Application.Services;

public class EvaluationService : IEvaluationService
{
    private readonly IEvaluationRepository _evaluationRepository;
    private readonly IStudentRepository _studentRepository;

    public EvaluationService(IEvaluationRepository evaluationRepository, IStudentRepository studentRepository)
    {
        _evaluationRepository = evaluationRepository;
        _studentRepository = studentRepository;
    }

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
