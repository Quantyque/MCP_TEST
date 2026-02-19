using FastAPI.Application.Interfaces;
using FastAPI.Domain.Entities;
using FastAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FastAPI.Infrastructure.Repositories;

public class EvaluationRepository : IEvaluationRepository
{
    private readonly ApplicationDbContext _context;

    public EvaluationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Evaluation evaluation)
    {
        await _context.Evaluations.AddAsync(evaluation);
    }

    public async Task<List<Evaluation>> GetByStudentIdAsync(Guid studentId)
    {
        return await _context.Evaluations
            .Include(e => e.Criteria)
            .Where(e => e.StudentId == studentId)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
