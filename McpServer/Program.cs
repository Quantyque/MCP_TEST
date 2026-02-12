using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using McpServer;

var builder = Host.CreateApplicationBuilder(args);

// Add dependencies
builder.Services.AddHttpClient();
builder.Services.AddSingleton<McpServer.Server>();

// Configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

builder.Logging.ClearProviders();
// builder.Logging.AddConsole(options => options.LogToStandardErrorThreshold = LogLevel.Information); // Removed to prevent stdout pollution

var app = builder.Build();

var server = app.Services.GetRequiredService<McpServer.Server>();
await server.RunAsync();
