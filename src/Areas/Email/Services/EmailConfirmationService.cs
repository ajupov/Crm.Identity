using System;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.MailSending.MailSender;
using Ajupov.Utils.All.Generator;
using Crm.Identity.Areas.Email.Settings;
using Crm.Identity.Areas.Identities.Models;
using Crm.Identity.Areas.Identities.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Crm.Identity.Areas.Email.Services
{
    public class EmailConfirmationService : IEmailConfirmationService
    {
        private readonly VerifyEmailSettings _settings;
        private readonly ILogger<EmailConfirmationService> _logger;
        private readonly IIdentitiesService _identitiesService;
        private readonly IIdentityTokensService _identityTokensService;
        private readonly IMailSender _mailSender;

        public EmailConfirmationService(
            IOptions<VerifyEmailSettings> settings,
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

        public async Task<Guid> SendMessageAsync(string email, string ipAddress, string userAgent, CancellationToken ct)
        {
            var now = DateTime.UtcNow;
            var code = Generator.GenerateAlphaNumericString(256);
            var identityTypes = new[] {IdentityType.EmailAndPassword};
            var identity = await _identitiesService.GetByKeyAndTypesAsync(email, identityTypes, ct);

            var token = new IdentityToken
            {
                IdentityId = identity.Id,
                Type = IdentityTokenType.EmailValidation,
                Value = code,
                CreateDateTime = now,
                ExpirationDateTime = now.AddDays(1),
                IpAddress = ipAddress,
                UserAgent = userAgent
            };

            var id = await _identityTokensService.CreateAsync(token, ct);
            await SendCodeAsync(email, id, code);

            return id;
        }

        private Task SendCodeAsync(string email, Guid tokenId, string code)
        {
            const string subject = "Email verification";
            var emails = new[] {email};
            var uri = string.Format(_settings.VerifyUriPattern, tokenId, code);
            var message = $"Click <a href='{uri}'>here</a> for verify email.";

            try
            {
                return _mailSender.SendAsync("No reply", _settings.FromAddress, subject, emails, true, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return Task.CompletedTask;
            }
        }
    }
}