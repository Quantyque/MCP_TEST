using FastAPI.Application.Interfaces;
using FastAPI.Domain.Entities;
using FastAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FastAPI.Infrastructure.Repositories;

public class BuildingRepository : IBuildingRepository
{
    private readonly ApplicationDbContext _context;

    public BuildingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Building>> GetAllAsync()
    {
        return await _context.Buildings.ToListAsync();
    }

    public async Task<Building?> GetByIdAsync(Guid id)
    {
        return await _context.Buildings.FindAsync(id);
    }

    public async Task AddAsync(Building building)
    {
        await _context.Buildings.AddAsync(building);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
