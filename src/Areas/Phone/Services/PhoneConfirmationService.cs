using System;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.SmsSending.SmsSender;
using Crm.Identity.Areas.Identities.Models;
using Crm.Identity.Areas.Identities.Services;
using Crm.Identity.Utils.Phone;

namespace Crm.Identity.Areas.Phone.Services
{
    public class PhoneConfirmationService : IPhoneConfirmationService
    {
        private readonly IIdentitiesService _identitiesService;
        private readonly IIdentityTokensService _identityTokensService;
        private readonly ISmsSender _smsSender;

        public PhoneConfirmationService(
            IIdentitiesService identitiesService,
            IIdentityTokensService identityTokensService,
            ISmsSender smsSender)
        {
            _identitiesService = identitiesService;
            _identityTokensService = identityTokensService;
            _smsSender = smsSender;
        }

        public async Task<Guid> SendMessageAsync(
            string country,
            string phone,
            string ipAddress,
            string userAgent,
            CancellationToken ct)
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

            var phoneWithPrefix = PhoneUtils.GetInternationalPrefix(country) + phone;

            await SendCodeAsync(phoneWithPrefix, code);

            return id;
        }

        private Task SendCodeAsync(string phone, string code)
        {
            var message = $"Confirmation code: {code}";

            return _smsSender.SendAsync(phone, message);
        }
    }
}