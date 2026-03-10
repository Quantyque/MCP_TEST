using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using McpServer.Models;

namespace McpServer.Services
{
    /// <summary>
    /// Service responsable de la communication avec l'API FastAPI et de la gestion de l'authentification.
    /// </summary>
    public class McpClientService
    {
        private readonly HttpClient _httpClient;
        private string _accessToken;
        private readonly string _baseUrl;

        public McpClientService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiBaseUrl"] ?? "http://localhost:5000";
        }

        private void EnsureAuthenticated()
        {
            if (!string.IsNullOrEmpty(_accessToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            }
        }

        /// <summary>
        /// Authentifie l'utilisateur et stocke le token d'accès.
        /// </summary>
        public async Task<TokenDto> LoginAsync(string email, string password)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/Auth/login", new LoginDto(email, password));
            response.EnsureSuccessStatusCode();
            var tokenDto = await response.Content.ReadFromJsonAsync<TokenDto>();
            if (tokenDto != null)
            {
                _accessToken = tokenDto.AccessToken;
            }
            return tokenDto;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            EnsureAuthenticated();
            return await _httpClient.GetFromJsonAsync<List<UserDto>>($"{_baseUrl}/api/Users");
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            EnsureAuthenticated();
            return await _httpClient.GetFromJsonAsync<UserDto>($"{_baseUrl}/api/Users/{id}");
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
        {
            EnsureAuthenticated();
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/Users", dto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UserDto>();
        }

        public async Task<List<BuildingDto>> GetAllBuildingsAsync()
        {
            EnsureAuthenticated();
            return await _httpClient.GetFromJsonAsync<List<BuildingDto>>($"{_baseUrl}/api/Buildings");
        }

        public async Task<BuildingDto> GetBuildingByIdAsync(Guid id)
        {
            EnsureAuthenticated();
            return await _httpClient.GetFromJsonAsync<BuildingDto>($"{_baseUrl}/api/Buildings/{id}");
        }

        public async Task<BuildingDto> CreateBuildingAsync(CreateBuildingDto dto)
        {
            EnsureAuthenticated();
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/Buildings", dto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<BuildingDto>();
        }

        public async Task<EvaluationDto> CreateEvaluationAsync(CreateEvaluationDto dto)
        {
            EnsureAuthenticated();
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/Evaluations", dto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<EvaluationDto>();
        }

        public async Task<object> GetAllCompetenciesAsync()
        {
            EnsureAuthenticated();
            // Retourne une liste anonyme ou dynamique
            return await _httpClient.GetFromJsonAsync<object>($"{_baseUrl}/api/Competencies");
        }
        
        public async Task<object> PingApiAsync()
        {
            // Health check often doesn't need auth
            return await _httpClient.GetFromJsonAsync<object>($"{_baseUrl}/api/Health"); // Assuming Health endpoint exists or similar
        }
    }
}
