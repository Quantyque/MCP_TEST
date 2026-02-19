using FastAPI.Domain.Entities;
using FastAPI.Domain.Enums;

namespace FastAPI.Application.DTOs;

public record UserDto(Guid Id, string FirstName, string LastName, string Email, ICollection<string> Roles);
public record CreateUserDto(string FirstName, string LastName, string Email, string Password);

public record BuildingDto(Guid Id, string Name, string Address);
public record CreateBuildingDto(string Name, string Address);

public record RoleDto(Guid Id, string Name, string Access);

public record EvaluationDto(Guid Id, string Name, string Description, string TeacherName, string StudentName, List<CriterionDto> Criteria);
public record CreateEvaluationDto(Guid StudentId, Guid BuildingId, string Name, string Description, List<CreateCriterionDto> Criteria);

public record CriterionDto(Guid Id, string Name, string Description, CompetenceType CompetenceType);
public record CreateCriterionDto(string Name, string Description, CompetenceType CompetenceType);

public record LoginDto(string Email, string Password);
public record TokenDto(string AccessToken, string RefreshToken);
