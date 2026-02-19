using System.ComponentModel;
using System.Text.Json;
using McpServer.Models;
using McpServer.Services;
using ModelContextProtocol.Server;

namespace McpServer.Tools
{
    [McpServerToolType]
    public class UserTools
    {
        private readonly McpClientService _clientService;

        public UserTools(McpClientService clientService)
        {
            _clientService = clientService;
        }

        [McpServerTool(Name = "get_users")]
        [Description("Récupère la liste de tous les utilisateurs.")]
        public async Task<string> GetUsers(CancellationToken cancellationToken)
        {
            var result = await _clientService.GetAllUsersAsync();
            return JsonSerializer.Serialize(result);
        }

        [McpServerTool(Name = "get_user")]
        [Description("Récupère un utilisateur par son ID.")]
        public async Task<string> GetUser(
            [Description("L'ID de l'utilisateur.")] Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _clientService.GetUserByIdAsync(id);
            return JsonSerializer.Serialize(result);
        }

        [McpServerTool(Name = "create_user")]
        [Description("Crée un nouvel utilisateur.")]
        public async Task<string> CreateUser(
            [Description("Le prénom de l'utilisateur.")] string firstName,
            [Description("Le nom de l'utilisateur.")] string lastName,
            [Description("L'email de l'utilisateur.")] string email,
            [Description("Le mot de passe.")] string password,
            CancellationToken cancellationToken)
        {
            var dto = new CreateUserDto(firstName, lastName, email, password);
            var result = await _clientService.CreateUserAsync(dto);
            return JsonSerializer.Serialize(result);
        }
    }
}
