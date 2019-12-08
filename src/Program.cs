using System.IO;
using System.Threading.Tasks;
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
using Crm.Identity.Areas.AccessTokens.Services;
using Crm.Identity.Areas.Claims.Services;
using Crm.Identity.Areas.Codes.Services;
using Crm.Identity.Areas.Email.Services;
using Crm.Identity.Areas.Email.Settings;
using Crm.Identity.Areas.Identities.Services;
using Crm.Identity.Areas.Identities.Storages;
using Crm.Identity.Areas.OAuth.Filters;
using Crm.Identity.Areas.OAuth.Options;
using Crm.Identity.Areas.OAuth.Services;
using Crm.Identity.Areas.OAuthClients.Services;
using Crm.Identity.Areas.OAuthClients.Storages;
using Crm.Identity.Areas.Password.Services;
using Crm.Identity.Areas.Password.Settings;
using Crm.Identity.Areas.Phone.Services;
using Crm.Identity.Areas.Profiles.Services;
using Crm.Identity.Areas.Profiles.Storages;
using Crm.Identity.Areas.RedirectUri.Services;
using Crm.Identity.Areas.RefreshTokens.Services;
using Crm.Identity.Areas.RefreshTokens.Storages;
using Crm.Identity.Areas.Registration.Services;
using Crm.Identity.Areas.Resources.Services;
using Crm.Identity.Areas.Resources.Storages;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Crm.Identity
{
    public static class Program
    {
        public static Task Main()
        {
            return ConfigurationExtensions.GetConfiguration()
                .ConfigureHost()
                .ConfigureLogging()
                .UseWebRoot(Directory.GetCurrentDirectory())
                .ConfigureServices((context, services) => services
                    .ConfigureMvc(typeof(ValidationFilter))
                    .ConfigureTracing()
                    .ConfigureApiDocumentation()
                    .ConfigureMetrics(context.Configuration)
                    .ConfigureMigrator(context.Configuration)
                    .ConfigureMailSending(context.Configuration)
                    .ConfigureSmsSending(context.Configuration)
                    .ConfigureOrm<OAuthClientsStorage>(context.Configuration)
                    .ConfigureOrm<IdentitiesStorage>(context.Configuration)
                    .ConfigureOrm<ProfilesStorage>(context.Configuration)
                    .ConfigureOrm<ResourcesStorage>(context.Configuration)
                    .ConfigureOrm<RefreshTokensStorage>(context.Configuration)
                    .ConfigureHotStorage(context.Configuration)
                    .Configure<VerifyEmailSettings>(context.Configuration.GetSection("VerifyEmailSettings"))
                    .Configure<ResetPasswordSettings>(context.Configuration.GetSection("ResetPasswordSettings"))
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
                .Configure((context, builder) =>
                {
                    if (context.HostingEnvironment.IsDevelopment())
                    {
                        builder.UseDeveloperExceptionPage();
                    }

                    builder.UseStaticFiles()
                        .UseApiDocumentationsMiddleware()
                        .UseMigrationsMiddleware()
                        .UseMetricsMiddleware()
                        .UseMvcMiddleware();
                })
                .Build()
                .RunAsync();
        }
    }
}