using System.ComponentModel;
using System.Text.Json;
using McpServer.Services;
using ModelContextProtocol.Server;

namespace McpServer.Tools
{
    [McpServerToolType]
    public class CompetencyTools
    {
        private readonly McpClientService _clientService;

        public CompetencyTools(McpClientService clientService)
        {
            _clientService = clientService;
        }

        [McpServerTool(Name = "list_competencies")]
        [Description("Liste les types de compétences disponibles.")]
        public async Task<string> ListCompetencies(CancellationToken cancellationToken)
        {
            var result = await _clientService.GetAllCompetenciesAsync();
            return JsonSerializer.Serialize(result);
        }
    }
}
