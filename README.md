# MCP_TEST Project

Ce dépôt contient deux solutions interconnectées pour tester l'architecture **Model Context Protocol (MCP)** avec une **API REST**.

## Structure du Projet

*   **FastAPI** (\`./FastAPI\`):
    *   **Description** : Une API Web ASP.NET Core (Clean Architecture) qui sert de "Backend" ou de système cible.
    *   **Rôle** : Simule votre application métier réelle (Gestion d'utilisateurs, bâtiments, évaluations).
    *   **Tech** : .NET 8, Entity Framework Core, PostgreSQL (ou In-Memory/SQLite pour test), Swagger.

*   **McpServer** (\`./McpServer\`):
    *   **Description** : Une application Console .NET 8 implémentant le protocole MCP.
    *   **Rôle** : Agit comme un **agent/interface** pour les LLM (Claude, etc.). Il transforme les requêtes de l'IA ("Crée un utilisateur") en appels API vers `FastAPI`.
    *   **Tech** : .NET 8, ModelContextProtocol.NET SDK.

## Prérequis

1.  **SDK .NET 8.0** ou supérieur.
2.  **Node.js & npm** (Requis uniquement pour lancer l'inspecteur de debug via `npx`).
3.  Une base de données PostgreSQL (si configurée dans `appsettings.json` de FastAPI), sinon assurez-vous que les migrations peuvent s'appliquer.

## Démarrage Rapide

### 1. Lancer l'API FastAPI
C'est le serveur qui doit tourner en fond.

```bash
cd FastAPI
dotnet run --project FastAPI.API
```
*L'API sera accessible sur `http://localhost:5000` (ou le port configuré).*

### 2. Lancer le Serveur MCP (Mode Debug / Inspecteur)
Pour tester les outils manuellement avec une interface Web.

Ouvrez un **nouveau terminal** :

```bash
cd McpServer
dotnet build
# Lancer l'exécutable généré directement (mode Debug)
.\bin\Debug\net8.0\win-x64\McpServer.exe
```

> **Note importante** : En mode `DEBUG`, l'application détecte qu'elle est lancée dans une console interactive. Elle lancera automatiquement l'inspecteur MCP (`npx @modelcontextprotocol/inspector`) qui ouvrira une fenêtre de navigateur pour tester les outils.

> **Note sur les Outils** : Les outils sont maintenant définis avec des attributs `[McpServerTool]`. Assurez-vous que le projet compile bien avant de lancer.

### 3. Utilisation avec un Client MCP (ex: Claude Desktop)
Configurez votre client pour exécuter l'exécutable compilé :

```json
{
  "mcpServers": {
    "my-csharp-server": {
      "command": "dotnet",
      "args": ["C:\\path\\to\\McpServer.dll"]
    }
  }
}
```

## Authentification
Le serveur MCP nécessite une authentification pour la plupart des actions.
1. Dans l'inspecteur ou via l'IA, utilisez l'outil `login` avec un email/mot de passe valide (ex: créés par le `DataSeeder` de FastAPI).
2. Le token sera stocké en mémoire pour la durée de la session du processus McpServer.

## Développement
*   Les outils sont définis dans `McpServer/Tools/`.
*   Le service client est dans `McpServer/Services/McpClientService.cs`.
