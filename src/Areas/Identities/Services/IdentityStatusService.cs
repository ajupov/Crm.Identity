using System.Threading;
using System.Threading.Tasks;
using Crm.Identity.Areas.Identities.Models;

namespace Crm.Identity.Areas.Identities.Services
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