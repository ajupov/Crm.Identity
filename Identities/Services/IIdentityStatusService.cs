using System.Threading;
using System.Threading.Tasks;

namespace Ajupov.Identity.Identities.Services
{
    public interface IIdentityStatusService
    {
        Task<bool> IsLoginExistsAsync(string login, CancellationToken ct);

        Task<bool> IsEmailExistsAsync(string email, CancellationToken ct);

        Task<bool> IsPhoneExistsAsync(string phone, CancellationToken ct);
    }
}