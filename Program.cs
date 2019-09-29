﻿using System.Threading.Tasks;
using Ajupov.Identity.Clients.Services;
using Ajupov.Identity.Clients.Storages;
using Ajupov.Identity.Identities.Services;
using Ajupov.Identity.Identities.Storages;
using Ajupov.Identity.OAuth.Filters;
using Ajupov.Identity.OAuth.Options;
using Ajupov.Identity.OAuth.Services;
using Ajupov.Identity.Profiles.Services;
using Ajupov.Identity.Profiles.Storages;
using Ajupov.Identity.Registration.Services;
using Ajupov.Identity.Registration.Settings;
using Ajupov.Infrastructure.All.ApiDocumentation;
using Ajupov.Infrastructure.All.Configuration;
using Ajupov.Infrastructure.All.Hosting;
using Ajupov.Infrastructure.All.HotStorage;
using Ajupov.Infrastructure.All.Logging;
using Ajupov.Infrastructure.All.MailSending;
using Ajupov.Infrastructure.All.Metrics;
using Ajupov.Infrastructure.All.Migrations;
using Ajupov.Infrastructure.All.Mvc;
using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.SmsSending;
using Ajupov.Infrastructure.All.Tracing;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Ajupov.Identity
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
                        .ConfigureMailSending(builder.Configuration)
                        .ConfigureSmsSending(builder.Configuration)
                        .ConfigureOrm<ClientsStorage>(builder.Configuration)
                        .ConfigureOrm<IdentitiesStorage>(builder.Configuration)
                        .ConfigureOrm<ProfilesStorage>(builder.Configuration)
                        .ConfigureHotStorage(builder.Configuration)
                        .Configure<VerifyEmailSettings>(builder.Configuration.GetSection("VerifyEmailSettings"))
                        .AddTransient<IClientsService, ClientsService>()
                        .AddTransient<IIdentitiesService, IdentitiesService>()
                        .AddTransient<IIdentityTokensService, IdentityTokensService>()
                        .AddTransient<IProfilesService, ProfilesService>()
                        .AddTransient<IOAuthService, OAuthService>()
                        .AddTransient<IRegistrationService, RegistrationService>()
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