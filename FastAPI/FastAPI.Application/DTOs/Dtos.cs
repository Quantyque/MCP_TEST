using FastAPI.Domain.Enums;

namespace FastAPI.Application.DTOs;

public record StudentDto(Guid Id, string FirstName, string LastName, string Address, List<EvaluationDto> Evaluations);
public record CreateStudentDto(string FirstName, string LastName, string Address);

public record EvaluationDto(Guid Id, string Name, string Description, List<CriterionDto> Criteria);
public record CreateEvaluationDto(Guid StudentId, string Name, string Description, List<CreateCriterionDto> Criteria);

public record CriterionDto(Guid Id, string Name, string Description, CompetenceType CompetenceType);
public record CreateCriterionDto(string Name, string Description, CompetenceType CompetenceType);
