# Crm.Identity
Identity server for [Lite CRM](https://litecrm.org)

## Usage
### Requirements
- [Docker](https://hub.docker.com/editions/community/docker-ce-desktop-windows)

### Steps
1. Run `posgres` in the Docker: `docker run --name postgres -p 5432:5432 -e POSTGRES_PASSWORD=postgres -d postgres`
2. Run `redis` in the Docker: `docker run --name redis -p 6379:6379 -d redis`
3. Clone file `appsettings.local-example.json` and rename to `appsettings.local.json`
4. Build and run application
5. The application will be run on https://localhost:3000

## Development
1. Clone this repository
2. Switch to a `new branch`
3. Make changes into `new branch`
4. Upgrade `PackageVersion` property value in `.csproj` file
5. Create pull request from `new branch` to `master` branch
6. Require code review
7. Merge pull request after approving
8. You can see image in [Github Packages](https://github.com/ajupov/Crm.Identity/packages)
