using System.Threading;
using System.Threading.Tasks;

namespace Crm.Identity.Areas.Identities.Services
{
    public interface IIdentityStatusService
    {
        Task<bool> IsLoginExistsAsync(string login, CancellationToken ct);

        Task<bool> IsEmailExistsAsync(string email, CancellationToken ct);

        Task<bool> IsPhoneExistsAsync(string phone, CancellationToken ct);
    }
}