namespace FastAPI.Domain.Entities;

public class Evaluation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Foreign Key
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;

    public ICollection<Criterion> Criteria { get; set; } = new List<Criterion>();
}
