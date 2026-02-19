using System;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace McpServer.Models
{
    public record LoginDto(string Email, string Password);
    public record TokenDto(string AccessToken, string RefreshToken);

    public record UserDto(Guid Id, string FirstName, string LastName, string Email, ICollection<string> Roles);
    public record CreateUserDto(string FirstName, string LastName, string Email, string Password);

    public record BuildingDto(Guid Id, string Name, string Address);
    public record CreateBuildingDto(string Name, string Address);

    public record EvaluationDto(Guid Id, string Name, string Description, string TeacherName, string StudentName, List<CriterionDto> Criteria);
    public record CreateEvaluationDto(Guid StudentId, Guid BuildingId, string Name, string Description, List<CreateCriterionDto> Criteria);

    public record CriterionDto(Guid Id, string Name, string Description, string CompetenceType);
    public record CreateCriterionDto(string Name, string Description, string CompetenceType);
    
    // Enum helper if needed or just use string for simplicity in MCP JSON
    public enum CompetenceType { SavoirEtre, SavoirFaire }
}
