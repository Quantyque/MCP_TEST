using McpServer.Services;
using McpServer.Tools;
using System.Diagnostics;

namespace McpServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
#if DEBUG
            // Si l'entrée n'est pas redirigée, on est lancé directement (pas par l'Inspector)
            // On lance donc l'Inspector qui va lui-même relancer ce process en subprocess
            if (!Console.IsInputRedirected)
            {
                var processPath = Environment.ProcessPath
                    ?? throw new InvalidOperationException("Impossible de déterminer le chemin de l'exécutable.");

                Console.WriteLine("DEBUG MODE: Lancement du MCP Inspector...");
                Console.WriteLine($"Executable: {processPath}");

                var psi = new ProcessStartInfo
                {
                    FileName = "cmd",
                    Arguments = $"/c npx @modelcontextprotocol/inspector \"{processPath}\"",
                    UseShellExecute = true,   // true = ouvre une nouvelle fenêtre cmd visible
                    CreateNoWindow = false
                };

                try
                {
                    Process.Start(psi);
                    // On quitte ce process : l'Inspector va relancer l'exe en subprocess
                    // avec stdin redirigé, donc Console.IsInputRedirected sera true
                    return;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Échec du lancement de l'Inspector : {ex.Message}");
                    Console.WriteLine("Appuie sur une touche pour quitter...");
                    Console.ReadKey();
                    return;
                }
            }

            Console.Error.WriteLine("DEBUG: Process lancé en tant que subprocess MCP (stdin redirigé).");
#endif

            var builder = Host.CreateApplicationBuilder(args);

            builder.Configuration
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

            builder.Services.AddLogging(configure =>
            {
#if DEBUG
                // En debug, on log uniquement sur stderr pour ne pas polluer stdout (protocole MCP)
                configure.AddConsole(options => options.LogToStandardErrorThreshold = LogLevel.Trace);
#else
                configure.AddConsole();
#endif
                configure.SetMinimumLevel(LogLevel.Debug);
            });

            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<McpClientService>();

            builder.Services
                .AddMcpServer()
                .WithStdioServerTransport()
                .WithTools<AuthTools>()
                .WithTools<UserTools>()
                .WithTools<BuildingTools>()
                .WithTools<EvaluationTools>()
                .WithTools<CompetencyTools>();

            await builder.Build().RunAsync();
        }
    }
}
