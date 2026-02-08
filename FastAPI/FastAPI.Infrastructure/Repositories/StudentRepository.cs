using FastAPI.Application.Interfaces;
using FastAPI.Domain.Entities;
using FastAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FastAPI.Infrastructure.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly ApplicationDbContext _context;

    public StudentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Student>> GetAllAsync()
    {
        return await _context.Students
            .Include(s => s.Evaluations)
            .ThenInclude(e => e.Criteria)
            .ToListAsync();
    }

    public async Task<Student?> GetByIdAsync(Guid id)
    {
        return await _context.Students
            .Include(s => s.Evaluations)
            .ThenInclude(e => e.Criteria)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task AddAsync(Student student)
    {
        await _context.Students.AddAsync(student);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
