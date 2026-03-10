using FastAPI.Application.Interfaces;
using FastAPI.Domain.Entities;
using FastAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FastAPI.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users
            .Include(u => u.BuildingRoles)
            .ThenInclude(br => br.Role)
            .Include(u => u.BuildingRoles)
            .ThenInclude(br => br.Building)
            .ToListAsync();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users
            .Include(u => u.BuildingRoles)
            .ThenInclude(br => br.Role)
            .Include(u => u.BuildingRoles)
            .ThenInclude(br => br.Building)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .Include(u => u.BuildingRoles)
            .ThenInclude(br => br.Role)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
