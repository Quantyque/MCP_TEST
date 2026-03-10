using FastAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastAPI.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Building> Buildings { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserBuildingRole> UserBuildingRoles { get; set; }
    public DbSet<Evaluation> Evaluations { get; set; }
    public DbSet<Criterion> Criteria { get; set; }
    public DbSet<AuthToken> AuthTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure UserBuildingRole (Many-to-Many with payload)
        modelBuilder.Entity<UserBuildingRole>()
            .HasOne(ubr => ubr.User)
            .WithMany(u => u.BuildingRoles)
            .HasForeignKey(ubr => ubr.UserId);

        modelBuilder.Entity<UserBuildingRole>()
            .HasOne(ubr => ubr.Building)
            .WithMany(b => b.UserRoles)
            .HasForeignKey(ubr => ubr.BuildingId);

        modelBuilder.Entity<UserBuildingRole>()
            .HasOne(ubr => ubr.Role)
            .WithMany()
            .HasForeignKey(ubr => ubr.RoleId);

        // Configure Evaluation relationships
        modelBuilder.Entity<Evaluation>()
            .HasOne(e => e.Student)
            .WithMany(u => u.ReceivedEvaluations)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

        modelBuilder.Entity<Evaluation>()
            .HasOne(e => e.Teacher)
            .WithMany(u => u.GivenEvaluations)
            .HasForeignKey(e => e.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Criterion
        modelBuilder.Entity<Criterion>()
            .HasOne(c => c.Evaluation)
            .WithMany(e => e.Criteria)
            .HasForeignKey(c => c.EvaluationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
