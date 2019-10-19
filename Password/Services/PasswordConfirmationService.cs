using System;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.Email.Services;
using Ajupov.Identity.Identities.Extensions;
using Ajupov.Identity.Identities.Models;
using Ajupov.Identity.Identities.Services;
using Ajupov.Identity.Password.Settings;
using Ajupov.Infrastructure.All.MailSending.MailSender;
using Infrastructure.All.Generator;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ajupov.Identity.Password.Services
{
    public class PasswordConfirmationService : IPasswordConfirmationService
    {
        private readonly ResetPasswordSettings _settings;
        private readonly ILogger<EmailConfirmationService> _logger;
        private readonly IIdentitiesService _identitiesService;
        private readonly IIdentityTokensService _identityTokensService;
        private readonly IMailSender _mailSender;

        public PasswordConfirmationService(
            IOptions<ResetPasswordSettings> settings,
            ILogger<EmailConfirmationService> logger,
            IIdentitiesService identitiesService,
            IIdentityTokensService identityTokensService,
            IMailSender mailSender)
        {
            _settings = settings.Value;
            _logger = logger;
            _identitiesService = identitiesService;
            _identityTokensService = identityTokensService;
            _mailSender = mailSender;
        }

        public async Task<Guid> SendMessageAsync(string login, string ipAddress, string userAgent, CancellationToken ct)
        {
            var now = DateTime.UtcNow;
            var code = Generator.GenerateAlphaNumericString(256);
            var identityTypes = IdentityTypeExtensions.TypesWithPassword;
            var identity = await _identitiesService.GetByKeyAndTypesAsync(login, identityTypes, ct);
            var emailIdentity = await _identitiesService.GetByProfileIdAndTypeAsync(identity.ProfileId,
                IdentityType.EmailAndPassword, ct);

            var token = new IdentityToken
            {
                IdentityId = emailIdentity.Id,
                Type = IdentityTokenType.PasswordReset,
                Value = code,
                CreateDateTime = now,
                ExpirationDateTime = now.AddDays(1),
                IpAddress = ipAddress,
                UserAgent = userAgent
            };

            var id = await _identityTokensService.CreateAsync(token, ct);
            await SendCodeAsync(login, id, code);

            return id;
        }

        private Task SendCodeAsync(string email, Guid tokenId, string code)
        {
            const string subject = "Password reset";
            var emails = new[] {email};
            var uri = string.Format(_settings.ResetUriPattern, tokenId, code);
            var message = $"Click <a href='{uri}'>here</a> for reset password.";

            try
            {
                return _mailSender.SendAsync(_settings.FromName, _settings.FromAddress, subject, emails, true, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return Task.CompletedTask;
            }
        }
    }
}