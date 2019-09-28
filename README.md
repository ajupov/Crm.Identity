# Ajupov.Infrastructure.All

All packages for backend applications.

##### Add Nuget source
```nuget source Add -Name "GitHub" -Source "https://nuget.pkg.github.com/ajupov/index.json" -UserName "ajupov" -Password "{API_KEY}"```

##### Set Nuget source ApiKey
```nuget setApiKey "{API_KEY}" -Source "GitHub"```

##### Pack
```dotnet pack --configuration Release```

##### Push
```nuget push ".\bin\Release\Ajupov.Infrastructure.All.{version}.nupkg" -Source "GitHub"```

##### Manage tokens
https://github.com/settings/tokens

##### Nuget.exe
https://www.nuget.org/downloads