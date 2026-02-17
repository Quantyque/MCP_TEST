using System.Text.Json;
using System.Text.Json.Nodes;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace McpServer;

public class Server
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<Server> _logger;
    private readonly string _apiBaseUrl;
    private readonly JsonSerializerOptions _jsonOptions = new() 
    { 
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull 
    };

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="Server"/>.
    /// </summary>
    /// <param name="httpClient">Le client HTTP pour effectuer des requêtes.</param>
    /// <param name="logger">Le logger pour l'application.</param>
    /// <param name="configuration">La configuration de l'application.</param>
    public Server(HttpClient httpClient, ILogger<Server> logger, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _apiBaseUrl = configuration["ApiBaseUrl"] ?? "http://localhost:5110";
    }

    /// <summary>
    /// Démarre le serveur et écoute les requêtes sur l'entrée standard (stdin).
    /// </summary>
    public async Task RunAsync()
    {
        _logger.LogInformation("MCP Server Started. Listening on Stdin.");
        
        using var stdin = Console.OpenStandardInput();
        // Set console output encoding to UTF8 to support emojis etc if needed, though default is usually fine.
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        using var reader = new StreamReader(stdin);
        
        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(line)) continue;

            try
            {
                var request = JsonSerializer.Deserialize<JsonRpcRequest>(line, _jsonOptions);
                if (request == null) continue;

                var response = await HandleRequestAsync(request);
                if (response != null)
                {
                    var responseJson = JsonSerializer.Serialize(response, _jsonOptions);
                    Console.WriteLine(responseJson);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing request");
            }
        }
    }

    /// <summary>
    /// Traite une requête JSON-RPC entrante.
    /// </summary>
    /// <param name="request">La requête JSON-RPC à traiter.</param>
    /// <returns>Une réponse JSON-RPC, ou null si aucune réponse n'est requise.</returns>
    private async Task<JsonRpcResponse?> HandleRequestAsync(JsonRpcRequest request)
    {
        try 
        {
            switch (request.Method)
            {
                case "initialize":
                    return new JsonRpcResponse 
                    { 
                        Jsonrpc = "2.0", 
                        Id = request.Id, 
                        Result = new 
                        { 
                            protocolVersion = "2024-11-05", 
                            capabilities = new { tools = new { } },
                            serverInfo = new { name = "StudentAssessmentMcp", version = "1.0.0" } 
                        } 
                    };
                case "notifications/initialized":
                    return null;
                case "tools/list":
                    return new JsonRpcResponse
                    {
                        Jsonrpc = "2.0",
                        Id = request.Id,
                        Result = new
                        {
                            tools = new object[]
                            {
                                new 
                                { 
                                    name = "ping_api", 
                                    description = "Checks API health and latency", 
                                    inputSchema = new { type = "object", properties = new { } } 
                                },
                                new 
                                { 
                                    name = "get_student", 
                                    description = "Retrieves student details by ID", 
                                    inputSchema = new 
                                    { 
                                        type = "object", 
                                        properties = new { studentId = new { type = "string", description = "The UUID of the student" } },
                                        required = new[] { "studentId" }
                                    } 
                                },
                                new 
                                { 
                                    name = "list_competencies", 
                                    description = "Lists all available competencies", 
                                    inputSchema = new { type = "object", properties = new { } } 
                                }
                            }
                        }
                    };
                case "tools/call":
                    return await HandleToolCallAsync(request);
                case "ping":
                    return new JsonRpcResponse { Jsonrpc = "2.0", Id = request.Id, Result = new { } };
                default:
                     return new JsonRpcResponse 
                    { 
                        Jsonrpc = "2.0", 
                        Id = request.Id, 
                        Error = new JsonRpcError { Code = -32601, Message = "Method not found" } 
                    };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Handler error");
            return new JsonRpcResponse 
            { 
                Jsonrpc = "2.0", 
                Id = request.Id, 
                Error = new JsonRpcError { Code = -32603, Message = "Internal error" } 
            };
        }
    }

    /// <summary>
    /// Gère l'appel d'un outil spécifique.
    /// </summary>
    /// <param name="request">La requête contenant les détails de l'appel d'outil.</param>
    /// <returns>La réponse de l'exécution de l'outil.</returns>
    private async Task<JsonRpcResponse> HandleToolCallAsync(JsonRpcRequest request)
    {
        if (request.Params is null) 
            return new JsonRpcResponse { Id = request.Id, Error = new JsonRpcError { Code = -32602, Message = "Missing params" } };
            
        var parameters = request.Params.Value;
        
        string name = "";
        if (parameters.ValueKind == JsonValueKind.Object && parameters.TryGetProperty("name", out var nameProp) && nameProp.ValueKind == JsonValueKind.String)
        {
            name = nameProp.GetString() ?? "";
        }
        else
        {
             return new JsonRpcResponse { Id = request.Id, Error = new JsonRpcError { Code = -32602, Message = "Missing tool name" } };
        }

        JsonElement args = default;
        if (parameters.TryGetProperty("arguments", out var argsProp))
        {
            args = argsProp;
        }

        try
        {
            object resultContent;

            switch (name)
            {
                case "ping_api":
                    resultContent = await PingApiAsync();
                    break;
                case "get_student":
                    string studentId = "";
                    if (args.ValueKind == JsonValueKind.Object && args.TryGetProperty("studentId", out var idProp))
                    {
                        studentId = idProp.GetString() ?? "";
                    }
                    
                    if (string.IsNullOrWhiteSpace(studentId)) 
                        throw new ArgumentException("studentId is required");
                        
                    resultContent = await GetStudentAsync(studentId);
                    break;
                case "list_competencies":
                    resultContent = await ListCompetenciesAsync();
                    break;
                default:
                    return new JsonRpcResponse 
                    { 
                        Jsonrpc = "2.0", 
                        Id = request.Id, 
                        Error = new JsonRpcError { Code = -32601, Message = "Tool not found" } 
                    };
            }

            return new JsonRpcResponse
            {
                Jsonrpc = "2.0",
                Id = request.Id,
                Result = new 
                { 
                    content = new[] 
                    { 
                        new { type = "text", text = JsonSerializer.Serialize(resultContent, _jsonOptions) } 
                    } 
                }
            };
        }
        catch (Exception ex)
        {
            return new JsonRpcResponse
            {
                Jsonrpc = "2.0",
                Id = request.Id,
                Result = new 
                { 
                    content = new[] 
                    { 
                        new { type = "text", text = $"Error: {ex.Message}" },
                        new { type = "text", text = $"Details: {ex.StackTrace}" } 
                    },
                    isError = true
                }
            };
        }
    }

    /// <summary>
    /// Vérifie l'état de santé de l'API.
    /// </summary>
    /// <returns>Un objet contenant l'état, le code de statut et la latence.</returns>
    private async Task<object> PingApiAsync()
    {
        var start = DateTime.UtcNow;
        var response = await _httpClient.GetAsync($"{_apiBaseUrl}/Health");
        var end = DateTime.UtcNow;
        
        response.EnsureSuccessStatusCode();

        return new
        {
            status = "ok",
            statusCode = (int)response.StatusCode,
            latencyMs = (end - start).TotalMilliseconds
        };
    }

    /// <summary>
    /// Récupère les informations d'un étudiant par son ID.
    /// </summary>
    /// <param name="studentId">L'identifiant de l'étudiant.</param>
    /// <returns>Les informations de l'étudiant.</returns>
    private async Task<object> GetStudentAsync(string studentId)
    {
        var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/Students/{studentId}");
        
        if (!response.IsSuccessStatusCode)
        {
             return new { error = $"API Error: {response.StatusCode}" };
        }

        return await response.Content.ReadFromJsonAsync<JsonElement?>() ?? throw new Exception("Empty JSON");
    }

    /// <summary>
    /// Récupère la liste des compétences.
    /// </summary>
    /// <returns>La liste des compétences.</returns>
    private async Task<object> ListCompetenciesAsync()
    {
        var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/Competencies");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<JsonElement>(); 
    }
}

/// <summary>
/// Représente une requête JSON-RPC.
/// </summary>
public class JsonRpcRequest
{
    [System.Text.Json.Serialization.JsonPropertyName("jsonrpc")]
    public string Jsonrpc { get; set; } = "2.0";
    [System.Text.Json.Serialization.JsonPropertyName("method")]
    public string Method { get; set; } = "";
    [System.Text.Json.Serialization.JsonPropertyName("id")]
    public object? Id { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("params")]
    public JsonElement? Params { get; set; }
}

/// <summary>
/// Représente une réponse JSON-RPC.
/// </summary>
public class JsonRpcResponse
{
    [System.Text.Json.Serialization.JsonPropertyName("jsonrpc")]
    public string Jsonrpc { get; set; } = "2.0";
    [System.Text.Json.Serialization.JsonPropertyName("id")]
    public object? Id { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("result")]
    public object? Result { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("error")]
    public JsonRpcError? Error { get; set; }
}

/// <summary>
/// Représente une erreur JSON-RPC.
/// </summary>
public class JsonRpcError
{
    [System.Text.Json.Serialization.JsonPropertyName("code")]
    public int Code { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("message")]
    public string Message { get; set; } = "";
}
