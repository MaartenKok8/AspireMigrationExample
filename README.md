# Aspire Migration Example
## About
This is the source code referenced by [this blog post series](https://maarten-kok.com/blog/migrating-an-existing-project-to-aspire-1). This explains how to integrate .NET Aspire into an already existing project.

## Running
Prerequisites:
- [Docker](https://docs.docker.com/engine/install/) or Docker Desktop
- [Dapr CLI](https://docs.dapr.io/getting-started/install-dapr-cli/)

Make sure these are installed. Then choose one of the following options:

### Dotnet run
Simply run the AppHost as a .NET project:
```
dotnet run --project src/AppHost/AppHost.csproj
```
This will download the dependencies, build the applications, and run them.

### Visual Studio / Rider
Open the solution, and choose the `AppHost` project as the startup project, and start debugging.

### Running integration tests
You can run the (integration) tests by executing:

```
dotnet test ./AspireMigrationExample.sln --configuration Release
```