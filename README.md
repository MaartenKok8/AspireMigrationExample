# Aspire Migration Example
## About
This is the source code referenced by [this blog post series](https://maarten-kok.com/blog/migrating-an-existing-project-to-aspire-1). This explains how to integrate .NET Aspire into an already existing project.

## Running
Make sure Docker and docker compose are [installed](https://docs.docker.com/compose/install/). Then choose one of the following options

### Docker compose
Simply run the following in the root of the repository:
```
docker compose up --wait
```
This will download the dependencies, build the applications, and run them.

### Visual Studio / Rider
Open the solution, and choose the `docker-compose` project as the startup project, and start debugging.

### Running integration tests
When having a running application, you can run the (integration) tests by executing:

```
dotnet test ./AspireMigrationExample.sln --configuration Release
```