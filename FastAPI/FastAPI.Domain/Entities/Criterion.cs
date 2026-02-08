using FastAPI.Domain.Enums;

namespace FastAPI.Domain.Entities;

public class Criterion
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public CompetenceType CompetenceType { get; set; }

    // Foreign Key
    public Guid EvaluationId { get; set; }
    public Evaluation Evaluation { get; set; } = null!;
}
