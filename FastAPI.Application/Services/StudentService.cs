using FastAPI.Application.DTOs;
using FastAPI.Application.Interfaces;
using FastAPI.Domain.Entities;

namespace FastAPI.Application.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;

    public StudentService(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<List<StudentDto>> GetAllAsync()
    {
        var students = await _studentRepository.GetAllAsync();
        return students.Select(MapToDto).ToList();
    }

    public async Task<StudentDto?> GetByIdAsync(Guid id)
    {
        var student = await _studentRepository.GetByIdAsync(id);
        return student == null ? null : MapToDto(student);
    }

    public async Task<StudentDto> CreateAsync(CreateStudentDto dto)
    {
        var student = new Student
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Address = dto.Address
        };

        await _studentRepository.AddAsync(student);
        await _studentRepository.SaveChangesAsync();

        return MapToDto(student);
    }

    private static StudentDto MapToDto(Student s) => new(
        s.Id,
        s.FirstName,
        s.LastName,
        s.Address,
        s.Evaluations.Select(e => new EvaluationDto(
            e.Id,
            e.Name,
            e.Description,
            e.Criteria.Select(c => new CriterionDto(c.Id, c.Name, c.Description, c.CompetenceType)).ToList()
        )).ToList()
    );
}
