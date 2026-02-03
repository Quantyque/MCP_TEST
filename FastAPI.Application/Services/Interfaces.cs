using FastAPI.Application.DTOs;

namespace FastAPI.Application.Services;

public interface IStudentService
{
    Task<List<StudentDto>> GetAllAsync();
    Task<StudentDto?> GetByIdAsync(Guid id);
    Task<StudentDto> CreateAsync(CreateStudentDto dto);
}

public interface IEvaluationService
{
    Task<EvaluationDto> AddEvaluationAsync(CreateEvaluationDto dto);
}
