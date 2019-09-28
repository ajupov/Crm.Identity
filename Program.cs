using System.Threading.Tasks;
using Ajupov.Infrastructure.All.ApiDocumentation;
using Ajupov.Infrastructure.All.Configuration;
using Ajupov.Infrastructure.All.Hosting;
using Ajupov.Infrastructure.All.HotStorage;
using Ajupov.Infrastructure.All.Logging;
using Ajupov.Infrastructure.All.Metrics;
using Ajupov.Infrastructure.All.Migrations;
using Ajupov.Infrastructure.All.Mvc;
using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.Tracing;
using Crm.Identity.Clients.Services;
using Crm.Identity.Clients.Storages;
using Crm.Identity.Identities.Services;
using Crm.Identity.Identities.Storages;
using Crm.Identity.OAuth.Filters;
using Crm.Identity.OAuth.Options;
using Crm.Identity.OAuth.Services;
using Crm.Identity.Profiles.Services;
using Crm.Identity.Profiles.Storages;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Crm.Identity
{
    public static class Program
    {
        public static Task Main()
        {
            return
                ConfigurationExtensions.GetConfiguration()
                    .ConfigureHost()
                    .ConfigureLogging()
                    .ConfigureServices((builder, services) => services
                        .ConfigureMvc(typeof(ValidationFilter))
                        .ConfigureTracing()
                        .ConfigureApiDocumentation()
                        .ConfigureMetrics(builder.Configuration)
                        .ConfigureMigrator(builder.Configuration)
                        .ConfigureOrm<ClientsStorage>(builder.Configuration)
                        .ConfigureOrm<IdentitiesStorage>(builder.Configuration)
                        .ConfigureOrm<ProfilesStorage>(builder.Configuration)
                        .ConfigureHotStorage(builder.Configuration)
                        .AddTransient<IClientsService, ClientsService>()
                        .AddTransient<IIdentitiesService, IdentitiesService>()
                        .AddTransient<IIdentityTokensService, IdentityTokensService>()
                        .AddTransient<IProfilesService, ProfilesService>()
                        .AddTransient<IOAuthService, OAuthService>()
                        .AddAuthentication(x =>
                        {
                            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                            x.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                            x.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
                            x.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
                            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        }).AddJwtBearer(options =>
                        {
                            options.RequireHttpsMetadata = false;
                            options.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateLifetime = true,
                                ValidateIssuer = true,
                                ValidateIssuerSigningKey = true,
                                ValidIssuer = AuthOptions.Issuer,
                                IssuerSigningKey = AuthOptions.GetKey(),
                                ValidateAudience = true,
                                ValidAudience = AuthOptions.Audience
                            };
                        }))
                    .Configure(builder => builder
                        .UseDeveloperExceptionPage()
                        .UseApiDocumentationsMiddleware()
                        .UseMigrationsMiddleware()
                        .UseMetricsMiddleware()
                        .UseMvcMiddleware())
                    .Build()
                    .RunAsync();
        }
    }
}