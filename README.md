# Crm.Identity

Identity server for https://litecrm.org

## Usage

### Requirements
- [Docker](https://hub.docker.com/editions/community/docker-ce-desktop-windows)

### Steps
1. Run `posgres` in the Docker: `docker run --name postgres -p 5432:5432 -e POSTGRES_PASSWORD=postgres -d postgres `
2. Run `redis` in the Docker: `docker run --name redis -p 6379:6379 -d redis`
3. Create `appsettings.local.json` file in the root of the repository with content:
```
{
  "ApplicationHost": "http://*:3000",
  "LoggingHost": "http://localhost:9200",
  "ConnectionStrings": {
    "MigrationsConnectionString": "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=postgres",
    "HotStorageConnectionString": "redis://id:redis@localhost:6379?ssl=false&db=1"
  },
  "OrmSettings": {
    "IsTestMode": false,
    "MainConnectionString": "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=postgres",
    "ReadonlyConnectionString": "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=postgres"
  },
  "JwtValidatorSettings": {
    "Audience": "",
    "SigningKey": "C59A10DF-962F-411F-A8B1-3362E43C2784"
  },
  "MailSendingSettings": {
    "IsTestMode": true
  },
  "SmsSendingSettings": {
    "IsTestMode": true
  },
  "VerifyEmailSettings": {
    "VerifyUriPattern": "http://localhost:3000/oauth/verifyemail?TokenId={0}&Code={1}"
  },
  "ResetPasswordSettings": {
    "ResetUriPattern": "http://localhost:3000/oauth/resetpasswordconfirmation?TokenId={0}&Code={1}"
  }
}
```

You can `build` and `run` the application

## Development
1. Clone this repository
2. Switch to a `new branch`
3. Make changes into `new branch`
4. Upgrade `PackageVersion` property value in `.csproj` file
5. Create pull request from `new branch` to `master` branch
6. Require code review
7. Merge pull request after approving
8. You can see image in [Github Packages](https://github.com/ajupov/Crm.Identity/packages)
