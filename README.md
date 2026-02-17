# Serveur MCP d'Évaluation Étudiante

Cette solution contient une implémentation C# d'un serveur **Model Context Protocol (MCP)** qui s'interface avec une API backend d'évaluation des étudiants.

## Structure du Projet

- **McpServer** : Application console .NET implémentant le protocole MCP (JSON-RPC sur Stdio).
- **FastAPI.API** : Backend API ASP.NET Core (fournit les données).

## Prérequis

- .NET 8.0/9.0 SDK
- Node.js (requis pour l'inspecteur MCP)

## Installation et Démarrage

Pour tester et utiliser le serveur MCP, vous devez lancer deux composants : l'API (Backend) et le Serveur MCP (via l'inspecteur).

### Étape 1 : Démarrer l'API

Le serveur MCP doit pouvoir communiquer avec l'API.

1. Ouvrez un terminal.
2. Démarrez l'API sur le port **5110** :
   ```powershell
   dotnet run --project FastAPI/FastAPI.API/FastAPI.API.csproj --urls=http://localhost:5110
   ```
3. Laissez ce terminal ouvert.

### Étape 2 : Démarrer l'Inspecteur MCP

L'inspecteur est l'interface client qui va "exécuter" votre serveur MCP et vous permettre d'interagir avec.

1. Ouvrez un **nouveau** terminal (en administrateur de préférence).
2. Vérifiez que le projet est bien compilé :
   ```powershell
   dotnet build McpServer/McpServer.csproj
   ```
3. Lancez l'inspecteur en utilisant le **chemin absolu** vers l'exécutable compilé (modifiez le chemin d'utilisateur si nécessaire) :
   ```bash
   npx @modelcontextprotocol/inspector "c:\Users\richa\source\repos\MCP_TEST\McpServer\bin\Debug\net9.0\McpServer.exe"
   ```
4. Attendez que l'URL (ex: `http://localhost:5173`) s'affiche et ouvrez-la dans votre navigateur.

### Étape 3 : Configuration de l'Interface Web

Une fois sur la page de l'inspecteur :

1. Vérifiez que **Transport Type** est sur `STDIO`.
2. Dans le champ **Command**, assurez-vous d'avoir le chemin absolu vers l'exécutable :
   `c:\Users\richa\source\repos\MCP_TEST\McpServer\bin\Debug\net9.0\McpServer.exe`
3. Le champ **Arguments** doit être **vide**.
4. Cliquez sur le bouton **`▷ Connect`**.

Le point "Disconnected" doit passer au vert ("Connected") et l'onglet **Tools** doit apparaître.

## Outils Disponibles

Une fois connecté, vous pouvez tester les outils suivants :

- **`ping_api`**
  - Vérifie la connectivité avec l'API backend.
  - *Retour attendu* : `{"status": "ok", "statusCode": 200, ...}`

- **`list_competencies`**
  - Récupère la liste des compétences définies dans l'API.
  - *Retour attendu* : Une liste JSON (ex: `[{"id": 0, "name": "SavoirEtre"}, ...]`).

- **`get_student`**
  - Récupère les détails d'un étudiant.
  - *Paramètre* : `studentId` (UUID de l'étudiant).
  - *Note* : Si l'étudiant n'existe pas, l'API renverra une erreur (ce qui confirme que la communication fonctionne).

## Configuration

Le serveur MCP est configuré via le fichier `McpServer/appsettings.json`.
- `ApiBaseUrl` : URL de l'API cible (actuellement configurée sur `http://localhost:5110`).

## Notes Techniques

- **Protocole** : Le serveur implémente une boucle JSON-RPC personnalisée sur l'entrée/sortie standard (stdio).
- **Logs** : La journalisation est redirigée (ou supprimée) de la sortie standard (stdout) pour ne pas corrompre le protocole JSON-RPC.
- **Sérialisation** : Utilise `System.Text.Json` configuré pour ignorer les valeurs nulles (évite les erreurs de validation strictes de l'inspecteur).
