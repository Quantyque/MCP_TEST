using FastAPI.Application.Interfaces;
using FastAPI.Application.Services;
using FastAPI.Infrastructure.Persistence;
using FastAPI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IEvaluationRepository, EvaluationRepository>();

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IEvaluationService, EvaluationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

// Auto-migrate on startup
// Auto-migrate on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    // Simple retry policy
    var retryCount = 0;
    while (retryCount < 5)
    {
        try 
        {
            Console.WriteLine($"Attempting migration (Try {retryCount + 1}/5)...");
            db.Database.Migrate();
            Console.WriteLine("Migration successful!");
            break;
        }
        catch (Exception ex)
        {
            retryCount++;
            Console.WriteLine($"Migration failed: {ex.Message}. Retrying in 5 seconds...");
            System.Threading.Thread.Sleep(5000);
        }
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
