# Crm.Identity
Identity server for [Lite CRM](https://litecrm.org)

## Usage
### Requirements
- [Docker](https://hub.docker.com/editions/community/docker-ce-desktop-windows)

### Steps
1. Run `posgres` in the Docker: `docker run --name postgres -p 5432:5432 -e POSTGRES_PASSWORD=postgres -d postgres`
2. Run `redis` in the Docker: `docker run --name redis -p 6379:6379 -d redis`
3. Connect to Postgres on `localhost:5432` and run script on database with name `postgres`:
    ```
    insert into "Resources" ("Id", "Name", "Scope", "Description", "Uri", "IsLocked", "IsDeleted", "CreateDateTime")
    values ('dc928c0c-dc5d-408e-a51f-253527d0c6fe', 'OpenID', 'openid', 'API', 'http://localhost:3000', false, false, now()),
           ('baf516c9-887d-4341-a3ab-72d310f0a449', 'Profile', 'profile', 'Profile', 'http://localhost:3000', false, false, now()),
           ('9e686e61-9392-4ae5-85c4-fc7ecece4dd1', 'API', 'api', 'API', 'http://localhost:9000', false, false, now())
    on conflict ("Id")
    do nothing;
    
    insert into "OAuthClients" ("Id", "ClientId", "ClientSecret", "RedirectUriPattern", "Audience", "IsLocked", "IsDeleted", "CreateDateTime")
    values ('83fa3c54-1bc2-4f49-852c-07c07784cc33', 'site-local', 'AAUtOjJMnMjj1gDucIltTuKI0dbeEtO4WMk7SPKV5aLffBlnxNuXcpPG3weH4IMHJbdouo1QdIIiski+KWlztz82uMecH1j4OytsS0UCqrGWnyFoalxZ5ZR14F2jjleD1QAvO9lrhjvtJdymrlT/hQW3xGDTdEkYbnDc2DgbHl+v3cY4KbS4CugKE9ut7zkoLcC7qD/gtddejixWpWTi/JYeRt8/ZA/P0hT2OHzNao996ZqFNiFIDIcK78hKk6mJ3EKNtRxQtHlZqSjRk+S2MDkRxHDCW4pyEznMRqsnjhUSiRk/OdCOPj6lPlikIiTmhK3cIsG3LAub+UFpT57wfQ==',
            'http://localhost:9000/Auth/Callback', 'localhost:9000', false, false, now())
    on conflict ("Id")
    do nothing;
    
    insert into "OAuthClientScopes" ("Id", "OAuthClientId", "Value")
    values ('68e1ad4b-6dfe-4ce9-86f0-167254956ae3', '83fa3c54-1bc2-4f49-852c-07c07784cc33', 'openid'),
           ('78b2146a-9716-43d0-b055-5d4ac7e38d67', '83fa3c54-1bc2-4f49-852c-07c07784cc33', 'profile'),
           ('bfb15c50-d5fc-4e03-b14e-eafc9e0001c8', '83fa3c54-1bc2-4f49-852c-07c07784cc33', 'api')
    on conflict ("Id")
    do nothing;
    ```
4. Build and run application
5. The application will be run on http://localhost:3000

## Development
1. Clone this repository
2. Switch to a `new branch`
3. Make changes into `new branch`
4. Upgrade `PackageVersion` property value in `.csproj` file
5. Create pull request from `new branch` to `master` branch
6. Require code review
7. Merge pull request after approving
8. You can see image in [Github Packages](https://github.com/ajupov/Crm.Identity/packages)
