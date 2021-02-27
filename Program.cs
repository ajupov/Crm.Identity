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
using Ajupov.Infrastructure.All.Mvc.Filters;
using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.SmsSending;
using Ajupov.Infrastructure.All.Tracing;
using Crm.Identity.AccessTokens.Services;
using Crm.Identity.CallbackUri.Services;
using Crm.Identity.Claims.Services;
using Crm.Identity.Codes.Services;
using Crm.Identity.Email.Services;
using Crm.Identity.Email.Settings;
using Crm.Identity.Identities.Services;
using Crm.Identity.Identities.Storages;
using Crm.Identity.OAuth.Services;
using Crm.Identity.OAuthClients.Services;
using Crm.Identity.OAuthClients.Storages;
using Crm.Identity.Password.Services;
using Crm.Identity.Password.Settings;
using Crm.Identity.Phone.Services;
using Crm.Identity.Profiles.Services;
using Crm.Identity.Profiles.Storages;
using Crm.Identity.RefreshTokens.Services;
using Crm.Identity.RefreshTokens.Storages;
using Crm.Identity.Registration.Services;
using Crm.Identity.Resources.Services;
using Crm.Identity.Resources.Storages;
using Crm.Identity.UserInfo.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Crm.Identity
{
    public static class Program
    {
        public static Task Main()
        {
            var configuration = Configuration.GetConfiguration();

            return configuration
                .ConfigureHosting()
                .ConfigureWebRoot()
                .ConfigureLogging(configuration)
                .ConfigureServices((context, services) =>
                {
                    services
                        .AddAuthorization()
                        .AddJwtAuthentication()
                        .AddJwtValidator(configuration);

                    services
                        .AddMvc(typeof(AutoValidateAntiforgeryTokenAttribute), typeof(ValidationFilter))
                        .AddJwtGenerator()
                        .AddJwtReader()
                        .AddTracing(configuration)
                        .AddApiDocumentation()
                        .AddMetrics(configuration)
                        .AddMigrator(configuration)
                        .AddMailSending(configuration)
                        .AddSmsSending(configuration)
                        .AddOrm<OAuthClientsStorage>(configuration)
                        .AddOrm<IdentitiesStorage>(configuration)
                        .AddOrm<ProfilesStorage>(configuration)
                        .AddOrm<ResourcesStorage>(configuration)
                        .AddOrm<RefreshTokensStorage>(configuration)
                        .AddHotStorage(configuration);

                    services
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
                        .AddTransient<IClaimsService, ClaimsService>()
                        .AddTransient<ICallbackUriService, CallbackUriService>()
                        .AddTransient<IUserInfoService, UserInfoService>();
                })
                .Configure((context, builder) =>
                {
                    if (context.HostingEnvironment.IsDevelopment())
                    {
                        builder.UseDeveloperExceptionPage()
                            .UseForwardedHeaders()
                            .UseHttpsRedirection()
                            .UseHsts();
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
