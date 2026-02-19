using System.ComponentModel;
using System.Text.Json;
using McpServer.Models;
using McpServer.Services;
using ModelContextProtocol.Server;

namespace McpServer.Tools
{
    [McpServerToolType]
    public class BuildingTools
    {
        private readonly McpClientService _clientService;

        public BuildingTools(McpClientService clientService)
        {
            _clientService = clientService;
        }

        [McpServerTool(Name = "list_buildings")]
        [Description("Liste tous les bâtiments.")]
        public async Task<string> ListBuildings(CancellationToken cancellationToken)
        {
            var result = await _clientService.GetAllBuildingsAsync();
            return JsonSerializer.Serialize(result);
        }

        [McpServerTool(Name = "get_building")]
        [Description("Récupère un bâtiment par son ID.")]
        public async Task<string> GetBuilding(
            [Description("L'ID du bâtiment.")] Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _clientService.GetBuildingByIdAsync(id);
            return JsonSerializer.Serialize(result);
        }

        [McpServerTool(Name = "create_building")]
        [Description("Crée un nouveau bâtiment.")]
        public async Task<string> CreateBuilding(
            [Description("Le nom du bâtiment.")] string name,
            [Description("L'adresse du bâtiment.")] string address,
            CancellationToken cancellationToken)
        {
            var dto = new CreateBuildingDto(name, address);
            var result = await _clientService.CreateBuildingAsync(dto);
            return JsonSerializer.Serialize(result);
        }
    }
}
