using FastAPI.API.Auth;
using FastAPI.Application.Interfaces;
using FastAPI.Application.Services;
using FastAPI.Infrastructure.Data;
using FastAPI.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Authentication
builder.Services.AddAuthentication("Bearer")
    .AddScheme<TokenAuthenticationSchemeOptions, TokenAuthenticationHandler>("Bearer", options => { });

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBuildingRepository, BuildingRepository>();
builder.Services.AddScoped<IEvaluationRepository, EvaluationRepository>();

// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBuildingService, BuildingService>();
builder.Services.AddScoped<IEvaluationService, EvaluationService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Data Seeder
builder.Services.AddScoped<DataSeeder>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

// Auto-migrate and Seed on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    
    // Simple retry policy
    var retryCount = 0;
    while (retryCount < 5)
    {
        try 
        {
            Console.WriteLine($"Attempting migration (Try {retryCount + 1}/5)...");
            db.Database.Migrate();
            Console.WriteLine("Migration successful!");
            
            Console.WriteLine("Seeding data...");
            await seeder.SeedAsync();
            Console.WriteLine("Data seeding successful!");
            
            break;
        }
        catch (Exception ex)
        {
            retryCount++;
            Console.WriteLine($"Migration/Seeding failed: {ex.Message}. Retrying in 5 seconds...");
            System.Threading.Thread.Sleep(5000);
        }
    }
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
