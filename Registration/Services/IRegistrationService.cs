using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ajupov.Identity.Registration.Services
{
    public interface IRegistrationService
    {
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
    }
}