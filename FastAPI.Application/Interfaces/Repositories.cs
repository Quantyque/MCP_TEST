using FastAPI.Domain.Entities;

namespace FastAPI.Application.Interfaces;

public interface IStudentRepository
{
    Task<List<Student>> GetAllAsync();
    Task<Student?> GetByIdAsync(Guid id);
    Task AddAsync(Student student);
    Task SaveChangesAsync();
}

public interface IEvaluationRepository
{
    Task AddAsync(Evaluation evaluation);
    Task SaveChangesAsync();
}
