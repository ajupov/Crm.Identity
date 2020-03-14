using System.Threading;
using System.Threading.Tasks;
using Crm.Identity.Identities.Models;
using Crm.Identity.Identities.Services;
using Crm.Identity.OAuth.Models.ChangePhone;
using Crm.Identity.Profiles.Services;

namespace Crm.Identity.Phone.Services
{
    public class PhoneChangeService : IPhoneChangeService
    {
        private readonly IProfilesService _profilesService;
        private readonly IIdentitiesService _identitiesService;
        private readonly IPhoneConfirmationService _phoneConfirmationService;

        public PhoneChangeService(
            IProfilesService profilesService,
            IIdentitiesService identitiesService,
            IPhoneConfirmationService phoneConfirmationService)
        {
            _profilesService = profilesService;
            _identitiesService = identitiesService;
            _phoneConfirmationService = phoneConfirmationService;
        }

        public async Task<PostChangePhoneResponse> ChangeAsync(
            string country,
            string oldPhone,
            string newPhone,
            string password,
            string ipAddress,
            string userAgent,
            CancellationToken ct)
        {
            var identityTypes = new[] {IdentityType.PhoneAndPassword};
            var identity = await _identitiesService.GetByKeyAndTypesAsync(oldPhone, identityTypes, ct);
            if (identity == null)
            {
                return new PostChangePhoneResponse(true);
            }

            var profile = await _profilesService.GetAsync(identity.ProfileId, ct);
            if (profile == null)
            {
                return new PostChangePhoneResponse(true);
            }

            var isPasswordCorrect = _identitiesService.IsPasswordCorrect(identity, password);
            if (!isPasswordCorrect)
            {
                return new PostChangePhoneResponse(true);
            }

            var newIdentity = new Identities.Models.Identity
            {
                Key = newPhone
            };

            await _identitiesService.UpdateAsync(identity, newIdentity, ct);
            var tokenId = await _phoneConfirmationService.SendMessageAsync(country, newPhone, ipAddress, userAgent, ct);

            return new PostChangePhoneResponse(tokenId);
        }
    }
}
