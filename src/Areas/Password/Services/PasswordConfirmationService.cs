using System;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.MailSending.MailSender;
using Ajupov.Utils.All.Generator;
using Crm.Identity.Areas.Email.Services;
using Crm.Identity.Areas.Identities.Extensions;
using Crm.Identity.Areas.Identities.Models;
using Crm.Identity.Areas.Identities.Services;
using Crm.Identity.Areas.Password.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Crm.Identity.Areas.Password.Services
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

        public async Task<Guid> SendMessageAsync(string key, string ipAddress, string userAgent, CancellationToken ct)
        {
            var now = DateTime.UtcNow;
            var code = Generator.GenerateAlphaNumericString(256);
            var identityTypes = IdentityTypeExtensions.TypesWithPassword;
            var identity = await _identitiesService.GetByKeyAndTypesAsync(key, identityTypes, ct);
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
            await SendCodeAsync(emailIdentity.Key, id, code);

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