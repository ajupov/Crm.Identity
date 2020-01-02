using System.Threading;
using System.Threading.Tasks;
using Crm.Identity.OAuth.Models.ChangeEmail;

namespace Crm.Identity.Email.Services
{
    public interface IEmailChangeService
    {
        Task<PostChangeEmailResponse> ChangeAsync(
            string oldEmail,
            string newEmail,
            string password,
            string ipAddress,
            string userAgent,
            CancellationToken ct);
    }
}