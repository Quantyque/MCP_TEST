using System.ComponentModel;
using System.Text.Json;
using McpServer.Models;
using McpServer.Services;
using ModelContextProtocol.Server;

namespace McpServer.Tools
{
    [McpServerToolType]
    public class EvaluationTools
    {
        private readonly McpClientService _clientService;

        public EvaluationTools(McpClientService clientService)
        {
            _clientService = clientService;
        }

        [McpServerTool(Name = "create_evaluation")]
        [Description("Crée une nouvelle évaluation.")]
        public async Task<string> CreateEvaluation(
            [Description("L'ID de l'étudiant.")] Guid studentId,
            [Description("L'ID du bâtiment.")] Guid buildingId,
            [Description("Le nom de l'évaluation.")] string name,
            [Description("La description de l'évaluation.")] string description,
            [Description("La liste des critères au format JSON : [{\"name\":\"...\",\"description\":\"...\",\"competenceType\":\"...\"}]")]
            string criteriaJson,
            CancellationToken cancellationToken)
        {
            var criteriaArgs = JsonSerializer.Deserialize<List<CreateCriterionArgs>>(criteriaJson)
                               ?? new List<CreateCriterionArgs>();

            var criteria = criteriaArgs
                .Select(c => new CreateCriterionDto(c.Name, c.Description, c.CompetenceType))
                .ToList();

            var dto = new CreateEvaluationDto(studentId, buildingId, name, description, criteria);
            var result = await _clientService.CreateEvaluationAsync(dto);
            return JsonSerializer.Serialize(result);
        }
    }

    // Classe helper pour la désérialisation du JSON des critères
    public record CreateCriterionArgs(string Name, string Description, string CompetenceType);
}
