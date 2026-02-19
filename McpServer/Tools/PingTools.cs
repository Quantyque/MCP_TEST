using McpServer.Services;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace McpServer.Tools
{
    [McpServerToolType]
    public class PingTools
    {
        private readonly McpClientService _clientService;

        public PingTools(McpClientService clientService)
        {
            _clientService = clientService;
        }

        [McpServerTool(Name = "ping_api")]
        [Description("Vérifie la disponibilité de l'API.")]
        public async Task<string> PingApi(CancellationToken cancellationToken)
        {
            var result = await _clientService.PingApiAsync();
            return System.Text.Json.JsonSerializer.Serialize(result);
        }
    }
}
