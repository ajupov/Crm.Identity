using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Identity.Identities.Models;
using Crm.Identity.Identities.Requests;
using Crm.Identity.Identities.Services;
using Crm.Identity.Profiles.Models;

namespace Crm.Identity.UserInfo.Services
{
    public class UserInfoService : IUserInfoService
    {
        private readonly IIdentitiesService _identitiesService;

        public UserInfoService(IIdentitiesService identitiesService)
        {
            _identitiesService = identitiesService;
        }

        public async Task<Models.UserInfo> GetByScopesAsync(Profile profile, CancellationToken ct)
        {
            var identityTypes = new List<IdentityType>
            {
                IdentityType.EmailAndPassword,
                IdentityType.PhoneAndPassword
            };

            var request = new IdentitiesGetPagedListRequest
            {
                ProfileId = profile.Id,
                Types = identityTypes,
                Limit = identityTypes.Count
            };

            var allIdentities = await _identitiesService.GetPagedListAsync(request, ct);
            var email = allIdentities.FirstOrDefault(x => x.Type == IdentityType.EmailAndPassword)?.Key;
            var phone = allIdentities.FirstOrDefault(x => x.Type == IdentityType.PhoneAndPassword)?.Key;

            return new Models.UserInfo
            {
                Id = profile.Id,
                Surname = profile.Surname,
                Name = profile.Name,
                Email = email,
                Phone = phone,
                Gender = profile.Gender?.ToString().ToLower(),
                BirthDate = profile.BirthDate?.ToString("dd.MM.yyyy")
            };
        }
    }
}