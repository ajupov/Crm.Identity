using System;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.SmsSending.SmsSender;
using Crm.Identity.Email.Services;
using Crm.Identity.Identities.Models;
using Crm.Identity.Identities.Services;
using Microsoft.Extensions.Logging;

namespace Crm.Identity.Phone.Services
{
    public class PhoneConfirmationService : IPhoneConfirmationService
    {
        private readonly ILogger<EmailConfirmationService> _logger;
        private readonly IIdentitiesService _identitiesService;
        private readonly IIdentityTokensService _identityTokensService;
        private readonly ISmsSender _smsSender;

        public PhoneConfirmationService(
            ILogger<EmailConfirmationService> logger,
            IIdentitiesService identitiesService,
            IIdentityTokensService identityTokensService,
            ISmsSender smsSender)
        {
            _logger = logger;
            _identitiesService = identitiesService;
            _identityTokensService = identityTokensService;
            _smsSender = smsSender;
        }

        public async Task<Guid> SendMessageAsync(string phone, string ipAddress, string userAgent, CancellationToken ct)
        {
            var now = DateTime.UtcNow;
            var code = new Random().Next(0, 9999).ToString("0000");
            var identityTypes = new[] {IdentityType.PhoneAndPassword};
            var identity = await _identitiesService.GetByKeyAndTypesAsync(phone, identityTypes, ct);

            var token = new IdentityToken
            {
                IdentityId = identity.Id,
                Type = IdentityTokenType.PhoneValidation,
                Value = code,
                CreateDateTime = now,
                ExpirationDateTime = now.AddMinutes(10),
                IpAddress = ipAddress,
                UserAgent = userAgent
            };

            var id = await _identityTokensService.CreateAsync(token, ct);
            await SendCodeAsync(phone, code);

            return id;
        }

        private Task SendCodeAsync(string phone, string code)
        {
            var message = $"Confirmation code - {code}.";

            try
            {
                return _smsSender.SendAsync(phone, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return Task.CompletedTask;
            }
        }
    }
}