using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Identity.Identities.Models;
using Crm.Identity.Identities.Services;

namespace Crm.Identity.Registration.Services
{
    public class RegistrationIdentityService : IRegistrationIdentityService
    {
        private readonly IIdentitiesService _identitiesService;

        public RegistrationIdentityService(IIdentitiesService identitiesService)
        {
            _identitiesService = identitiesService;
        }

        public async Task CreateLoginIdentityAsync(
            Guid profileId,
            string login,
            string passwordHash,
            CancellationToken ct)
        {
            var loginIdentity = new Identities.Models.Identity
            {
                ProfileId = profileId,
                Type = IdentityType.LoginAndPassword,
                Key = login,
                PasswordHash = passwordHash,
                IsVerified = true
            };

            await _identitiesService.CreateAsync(loginIdentity, ct);
        }

        public async Task CreateEmailIdentityAsync(
            Guid profileId,
            string email,
            string passwordHash,
            CancellationToken ct)
        {
            var emailIdentity = new Identities.Models.Identity
            {
                ProfileId = profileId,
                Type = IdentityType.EmailAndPassword,
                Key = email,
                PasswordHash = passwordHash,
                IsVerified = false
            };

            await _identitiesService.CreateAsync(emailIdentity, ct);
        }

        public async Task CreatePhoneIdentityAsync(
            Guid profileId,
            string phone,
            string passwordHash,
            CancellationToken ct)
        {
            var phoneIdentity = new Identities.Models.Identity
            {
                ProfileId = profileId,
                Type = IdentityType.PhoneAndPassword,
                Key = phone,
                PasswordHash = passwordHash,
                IsVerified = false
            };

            await _identitiesService.CreateAsync(phoneIdentity, ct);
        }
    }
}