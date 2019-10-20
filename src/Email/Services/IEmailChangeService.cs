using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.src.OAuth.Models.ChangeEmail;

namespace Ajupov.Identity.src.Email.Services
{
    public interface IEmailChangeService
    {
        Task<PostChangeEmailResponse> ChangeAsync(
            string oldEmail,
            string newEmail,
            string password,
            string userAgent,
            string ipAddress,
            CancellationToken ct);
    }
}