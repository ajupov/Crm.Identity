using System.IO;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.ApiDocumentation;
using Ajupov.Infrastructure.All.Configuration;
using Ajupov.Infrastructure.All.Hosting;
using Ajupov.Infrastructure.All.HotStorage;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Infrastructure.All.Logging;
using Ajupov.Infrastructure.All.MailSending;
using Ajupov.Infrastructure.All.Metrics;
using Ajupov.Infrastructure.All.Migrations;
using Ajupov.Infrastructure.All.Mvc;
using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.SmsSending;
using Ajupov.Infrastructure.All.Tracing;
using Crm.Identity.AccessTokens;
using Crm.Identity.AccessTokens.Services;
using Crm.Identity.CallbackUri.Services;
using Crm.Identity.Claims.Services;
using Crm.Identity.Codes.Services;
using Crm.Identity.Email.Services;
using Crm.Identity.Email.Settings;
using Crm.Identity.Identities.Services;
using Crm.Identity.Identities.Storages;
using Crm.Identity.OAuth.Filters;
using Crm.Identity.OAuth.Services;
using Crm.Identity.OAuthClients.Services;
using Crm.Identity.OAuthClients.Storages;
using Crm.Identity.Password.Services;
using Crm.Identity.Password.Settings;
using Crm.Identity.Phone.Services;
using Crm.Identity.Profiles.Services;
using Crm.Identity.Profiles.Storages;
using Crm.Identity.RedirectUri.Services;
using Crm.Identity.RefreshTokens.Services;
using Crm.Identity.RefreshTokens.Storages;
using Crm.Identity.Registration.Services;
using Crm.Identity.Resources.Services;
using Crm.Identity.Resources.Storages;
using Crm.Identity.UserInfo.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Crm.Identity
{
    public static class Program
    {
        public static Task Main()
        {
            var configuration = ConfigurationExtensions.GetConfiguration();

            return configuration
                .ConfigureHost()
                .ConfigureLogging(configuration)
                .UseWebRoot(Directory.GetCurrentDirectory())
                .ConfigureServices((context, services) =>
                {
                    services
                        .AddAuthorization()
                        .AddJwtAuthentication()
                        .AddJwtValidator(AccessTokenDefaults.SigningKey);

                    services
                        .ConfigureMvc(typeof(ValidationFilter))
                        .ConfigureJwtGenerator()
                        .ConfigureJwtReader()
                        .ConfigureTracing(configuration)
                        .ConfigureApiDocumentation()
                        .ConfigureMetrics(configuration)
                        .ConfigureMigrator(configuration)
                        .ConfigureMailSending(configuration)
                        .ConfigureSmsSending(configuration)
                        .ConfigureOrm<OAuthClientsStorage>(configuration)
                        .ConfigureOrm<IdentitiesStorage>(configuration)
                        .ConfigureOrm<ProfilesStorage>(configuration)
                        .ConfigureOrm<ResourcesStorage>(configuration)
                        .ConfigureOrm<RefreshTokensStorage>(configuration)
                        .ConfigureHotStorage(configuration)
                        .Configure<VerifyEmailSettings>(configuration.GetSection(nameof(VerifyEmailSettings)))
                        .Configure<ResetPasswordSettings>(configuration.GetSection(nameof(ResetPasswordSettings)));

                    services
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
                        .AddTransient<IUserInfoService, UserInfoService>();
                })
                .Configure((context, builder) =>
                {
                    if (context.HostingEnvironment.IsDevelopment())
                    {
                        builder.UseDeveloperExceptionPage();
                    }

                    builder
                        .UseStaticFiles()
                        .UseApiDocumentationsMiddleware()
                        .UseMigrationsMiddleware()
                        .UseMetricsMiddleware()
                        .UseAuthentication()
                        .UseAuthorization()
                        .UseMvcMiddleware();
                })
                .Build()
                .RunAsync();
        }
    }
}