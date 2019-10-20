using System.IO;
using System.Threading.Tasks;
using Ajupov.Identity.src.AccessTokens.Services;
using Ajupov.Identity.src.Claims.Services;
using Ajupov.Identity.src.Codes.Services;
using Ajupov.Identity.src.Email.Services;
using Ajupov.Identity.src.Email.Settings;
using Ajupov.Identity.src.Identities.Services;
using Ajupov.Identity.src.Identities.Storages;
using Ajupov.Identity.src.OAuth.Filters;
using Ajupov.Identity.src.OAuth.Options;
using Ajupov.Identity.src.OAuth.Services;
using Ajupov.Identity.src.OAuthClients.Services;
using Ajupov.Identity.src.OAuthClients.Storages;
using Ajupov.Identity.src.Password.Services;
using Ajupov.Identity.src.Password.Settings;
using Ajupov.Identity.src.Phone.Services;
using Ajupov.Identity.src.Profiles.Services;
using Ajupov.Identity.src.Profiles.Storages;
using Ajupov.Identity.src.RedirectUri.Services;
using Ajupov.Identity.src.RefreshTokens.Services;
using Ajupov.Identity.src.RefreshTokens.Storages;
using Ajupov.Identity.src.Registration.Services;
using Ajupov.Identity.src.Resources.Services;
using Ajupov.Identity.src.Resources.Storages;
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

namespace Ajupov.Identity.src
{
    public static class Program
    {
        public static Task Main()
        {
            return
                ConfigurationExtensions.GetConfiguration()
                    .ConfigureHost()
                    .ConfigureLogging()
                    .UseWebRoot(Directory.GetCurrentDirectory())
                    .ConfigureServices((builder, services) => services
                        .ConfigureMvc(typeof(ValidationFilter))
                        .ConfigureTracing()
                        .ConfigureApiDocumentation()
                        .ConfigureMetrics(builder.Configuration)
                        .ConfigureMigrator(builder.Configuration)
                        .ConfigureMailSending(builder.Configuration)
                        .ConfigureSmsSending(builder.Configuration)
                        .ConfigureOrm<OAuthClientsStorage>(builder.Configuration)
                        .ConfigureOrm<IdentitiesStorage>(builder.Configuration)
                        .ConfigureOrm<ProfilesStorage>(builder.Configuration)
                        .ConfigureOrm<ResourcesStorage>(builder.Configuration)
                        .ConfigureOrm<RefreshTokensStorage>(builder.Configuration)
                        .ConfigureHotStorage(builder.Configuration)
                        .Configure<VerifyEmailSettings>(builder.Configuration.GetSection("VerifyEmailSettings"))
                        .Configure<ResetPasswordSettings>(builder.Configuration.GetSection("ResetPasswordSettings"))
                        .AddTransient<IOAuthService, OAuthService>()
                        .AddTransient<IRegistrationService, RegistrationService>()
                        .AddTransient<IRegistrationIdentityService, RegistrationIdentityService>()
                        .AddTransient<IEmailChangeService, EmailChangeService>()
                        .AddTransient<IEmailConfirmationService, EmailConfirmationService>()
                        .AddTransient<IEmailVerificationService, EmailVerificationService>()
                        .AddTransient<IPhoneChangeService, PhoneChangeService>()
                        .AddTransient<IPhoneConfirmationService, PhoneConfirmationService>()
                        .AddTransient<IPhoneVerificationService, PhoneVerificationService>()
                        .AddTransient<IPasswordChangeService, PasswordChangeService>()
                        .AddTransient<IPasswordResetService, PasswordResetService>()
                        .AddTransient<IPasswordConfirmationService, PasswordConfirmationService>()
                        .AddTransient<IOAuthClientsService, OAuthClientsService>()
                        .AddTransient<IIdentitiesService, IdentitiesService>()
                        .AddTransient<IIdentityTokensService, IdentityTokensService>()
                        .AddTransient<IIdentityStatusService, IdentityStatusService>()
                        .AddTransient<IProfilesService, ProfilesService>()
                        .AddTransient<ICodesService, CodesService>()
                        .AddTransient<IAccessTokensService, AccessTokensService>()
                        .AddTransient<IRefreshTokensService, RefreshTokensService>()
                        .AddTransient<IResourcesService, ResourcesService>()
                        .AddTransient<IScopeRolesService, ScopeRolesService>()
                        .AddTransient<IClaimsService, ClaimsService>()
                        .AddTransient<ICallbackUriService, CallbackUriService>()
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
                        .UseStaticFiles()
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