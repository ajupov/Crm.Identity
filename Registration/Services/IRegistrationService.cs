using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ajupov.Identity.Registration.Services
{
    public interface IRegistrationService
    {
        Task<bool> IsLoginExistsAsync(string login, CancellationToken ct);

        Task<bool> IsEmailExistsAsync(string email, CancellationToken ct);

        Task<bool> IsPhoneExistsAsync(string phone, CancellationToken ct);

        Task<Guid> RegisterAsync(
            string surname,
            string name,
            string login,
            string email,
            string phone,
            string password,
            string ipAddress,
            string userAgent,
            CancellationToken ct);

        Task SendEmailConfirmationEmailAsync(string email, string ipAddress, string userAgent, CancellationToken ct);

        Task<Guid> SendPhoneConfirmationSmsAsync(string phone, string ipAddress, string userAgent, CancellationToken ct);

        Task<bool> VerifyEmailAsync(Guid tokenId, string code, CancellationToken ct);

        Task<bool> VerifyPhoneAsync(Guid tokenId, string code, CancellationToken ct);
    }
}