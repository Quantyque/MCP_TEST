using Bogus;
using FastAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastAPI.Infrastructure.Data;

public class DataSeeder
{
    private readonly ApplicationDbContext _context;

    public DataSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        if (await _context.Buildings.AnyAsync()) return; // Already seeded

        var faker = new Faker("fr");

        // 1. Create Roles
        var roles = new List<Role>
        {
            new Role { Name = "Student", Access = "Read-Only" },
            new Role { Name = "Teacher", Access = "Read-Write" },
            new Role { Name = "Admin", Access = "Full-Access" }
        };
        await _context.Roles.AddRangeAsync(roles);
        await _context.SaveChangesAsync();

        var studentRole = roles.First(r => r.Name == "Student");
        var teacherRole = roles.First(r => r.Name == "Teacher");

        // 2. Create Buildings
        var buildings = new Faker<Building>()
            .RuleFor(b => b.Name, f => f.Company.CompanyName() + " Campus")
            .RuleFor(b => b.Address, f => f.Address.FullAddress())
            .Generate(3);
        
        await _context.Buildings.AddRangeAsync(buildings);
        await _context.SaveChangesAsync();

        // 3. Create Users
        var users = new Faker<User>()
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
            .RuleFor(u => u.PasswordHash, f => "password") // Simple password
            .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
            .Generate(50);

        await _context.Users.AddRangeAsync(users);
        await _context.SaveChangesAsync();

        // 4. Assign Roles to Users in Buildings
        var userBuildingRoles = new List<UserBuildingRole>();
        var random = new Random();

        foreach (var user in users)
        {
            // Assign random role in random building
            var building = buildings[random.Next(buildings.Count)];
            var role = random.Next(2) == 0 ? studentRole : teacherRole;

            userBuildingRoles.Add(new UserBuildingRole
            {
                UserId = user.Id,
                BuildingId = building.Id,
                RoleId = role.Id
            });

            // Some users have roles in multiple buildings
            if (random.Next(3) == 0) // 33% chance
            {
                var otherBuilding = buildings.First(b => b.Id != building.Id);
                var otherRole = role.Name == "Student" ? teacherRole : studentRole; // Context switch

                userBuildingRoles.Add(new UserBuildingRole
                {
                    UserId = user.Id,
                    BuildingId = otherBuilding.Id,
                    RoleId = otherRole.Id
                });
            }
        }

        await _context.UserBuildingRoles.AddRangeAsync(userBuildingRoles);
        await _context.SaveChangesAsync();
    }
}
