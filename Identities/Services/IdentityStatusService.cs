using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.Identities.Models;

namespace Ajupov.Identity.Identities.Services
{
    public class IdentityStatusService : IIdentityStatusService
    {
        private readonly IIdentitiesService _identitiesService;

        public IdentityStatusService(IIdentitiesService identitiesService)
        {
            _identitiesService = identitiesService;
        }

        public Task<bool> IsLoginExistsAsync(string login, CancellationToken ct)
        {
            return _identitiesService.IsExistByKeyAndTypeAsync(login, IdentityType.LoginAndPassword, ct);
        }

        public Task<bool> IsEmailExistsAsync(string email, CancellationToken ct)
        {
            return _identitiesService.IsExistByKeyAndTypeAsync(email, IdentityType.EmailAndPassword, ct);
        }

        public Task<bool> IsPhoneExistsAsync(string phone, CancellationToken ct)
        {
            return _identitiesService.IsExistByKeyAndTypeAsync(phone, IdentityType.PhoneAndPassword, ct);
        }
    }
}