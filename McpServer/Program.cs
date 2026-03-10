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
            // Fichier de log pour capturer tout ce qui se passe
            var logPath = Path.Combine(AppContext.BaseDirectory, "debug.log");
            File.WriteAllText(logPath, $"[{DateTime.Now}] Démarrage. Args: {string.Join(", ", args)}\n");

            bool isSubprocess = args.Contains("--mcp-subprocess");
            File.AppendAllText(logPath, $"[{DateTime.Now}] isSubprocess: {isSubprocess}\n");

            if (!isSubprocess)
            {
                var processPath = Environment.ProcessPath;
                File.AppendAllText(logPath, $"[{DateTime.Now}] ProcessPath: {processPath}\n");

                if (processPath == null)
                {
                    File.AppendAllText(logPath, $"[{DateTime.Now}] ERREUR: ProcessPath est null\n");
                    Console.WriteLine("ERREUR: Impossible de déterminer le chemin de l'exécutable.");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("DEBUG MODE: Lancement du MCP Inspector...");
                Console.WriteLine($"Executable: {processPath}");

                var psi = new ProcessStartInfo
                {
                    FileName = "cmd",
                    Arguments = $"/k npx @modelcontextprotocol/inspector \"{processPath}\" --mcp-subprocess",
                    UseShellExecute = true,
                    CreateNoWindow = false
                };

                try
                {
                    File.AppendAllText(logPath, $"[{DateTime.Now}] Tentative de lancement de l'Inspector...\n");
                    var process = Process.Start(psi);
                    File.AppendAllText(logPath, $"[{DateTime.Now}] Process lancé: {process?.Id}\n");

                    Console.WriteLine("Inspector lancé. Appuie sur une touche pour fermer cette fenêtre...");
                    Console.ReadKey(); // Garde la fenêtre ouverte pour voir les messages
                    return;
                }
                catch (Exception ex)
                {
                    var msg = $"[{DateTime.Now}] ERREUR lancement Inspector: {ex}\n";
                    File.AppendAllText(logPath, msg);
                    Console.Error.WriteLine(msg);
                    Console.WriteLine("Appuie sur une touche pour quitter...");
                    Console.ReadKey();
                    return;
                }
            }

            File.AppendAllText(logPath, $"[{DateTime.Now}] Démarrage en mode subprocess MCP\n");
            Console.Error.WriteLine("DEBUG: Subprocess MCP démarré.");
#endif

            try
            {
                var builder = Host.CreateApplicationBuilder(args);

                builder.Configuration
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

                builder.Services.AddLogging(configure =>
                {
#if DEBUG
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

#if DEBUG
                File.AppendAllText(logPath, $"[{DateTime.Now}] Services configurés, démarrage du host...\n");
#endif
                await builder.Build().RunAsync();
            }
            catch (Exception ex)
            {
#if DEBUG
                var logPath2 = Path.Combine(AppContext.BaseDirectory, "debug.log");
                File.AppendAllText(logPath2, $"[{DateTime.Now}] EXCEPTION: {ex}\n");
#endif
                Console.Error.WriteLine($"EXCEPTION FATALE: {ex}");
                Console.ReadKey();
            }
        }
    }
}