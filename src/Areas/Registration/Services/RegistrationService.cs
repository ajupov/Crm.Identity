using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Identity.Areas.Email.Services;
using Crm.Identity.Areas.Phone.Services;
using Crm.Identity.Areas.Profiles.Models;
using Crm.Identity.Areas.Profiles.Services;
using PasswordUtils = Ajupov.Utils.All.Password.Password;

namespace Crm.Identity.Areas.Registration.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IProfilesService _profilesService;
        private readonly IRegistrationIdentityService _registrationIdentityService;
        private readonly IEmailConfirmationService _emailConfirmationService;
        private readonly IPhoneConfirmationService _phoneConfirmationService;

        public RegistrationService(
            IProfilesService profilesService,
            IRegistrationIdentityService registrationIdentityService,
            IEmailConfirmationService emailConfirmationService,
            IPhoneConfirmationService phoneConfirmationService)
        {
            _profilesService = profilesService;
            _registrationIdentityService = registrationIdentityService;
            _emailConfirmationService = emailConfirmationService;
            _phoneConfirmationService = phoneConfirmationService;
        }

        public async Task<Guid> RegisterAsync(
            string surname,
            string name,
            string login,
            string email,
            string phone,
            string password,
            string ipAddress,
            string userAgent,
            CancellationToken ct)
        {
            var profile = new Profile
            {
                Surname = surname,
                Name = name
            };

            profile.Id = await _profilesService.CreateAsync(profile, ct);

            var passwordHash = PasswordUtils.ToPasswordHash(password);

            await _registrationIdentityService.CreateLoginIdentityAsync(profile.Id, login, passwordHash, ct);
            await _registrationIdentityService.CreateEmailIdentityAsync(profile.Id, email, passwordHash, ct);
            await _registrationIdentityService.CreatePhoneIdentityAsync(profile.Id, phone, passwordHash, ct);

            await _emailConfirmationService.SendMessageAsync(email, ipAddress, userAgent, ct);
            var tokenId = await _phoneConfirmationService.SendMessageAsync(phone, ipAddress, userAgent, ct);

            return tokenId;
        }
    }
}