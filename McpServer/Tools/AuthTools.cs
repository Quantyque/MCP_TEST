using System.ComponentModel;
using McpServer.Services;
using ModelContextProtocol.Server;

namespace McpServer.Tools
{
    [McpServerToolType]
    public class AuthTools
    {
        private readonly McpClientService _clientService;

        public AuthTools(McpClientService clientService)
        {
            _clientService = clientService;
        }

        [McpServerTool(Name = "login")]
        [Description("Authentifie l'utilisateur et initialise la session avec le token JWT.")]
        public async Task<string> Login(
            [Description("L'email de l'utilisateur.")] string email,
            [Description("Le mot de passe de l'utilisateur.")] string password,
            CancellationToken cancellationToken)
        {
            var result = await _clientService.LoginAsync(email, password);
            return $"Authentification réussie. Token: {result.AccessToken[..10]}...";
        }
    }
}
