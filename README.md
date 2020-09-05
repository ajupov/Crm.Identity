# Crm.Identity

Identity server for https://litecrm.org

## Usage

### Requirements
- [Docker](https://hub.docker.com/editions/community/docker-ce-desktop-windows)

### Steps
1. Run `posgres` in the Docker: `docker run --name postgres -p 5432:5432 -e POSTGRES_PASSWORD=postgres -d postgres `
2. Run `redis` in the Docker: `docker run --name redis -p 6379:6379 -d redis`
3. Build and run application
4. Application will be started on http://localhost:3000

## Development
1. Clone this repository
2. Switch to a `new branch`
3. Make changes into `new branch`
4. Upgrade `PackageVersion` property value in `.csproj` file
5. Create pull request from `new branch` to `master` branch
6. Require code review
7. Merge pull request after approving
8. You can see image in [Github Packages](https://github.com/ajupov/Crm.Identity/packages)
