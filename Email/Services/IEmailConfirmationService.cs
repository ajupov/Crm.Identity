using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ajupov.Identity.Email.Services
{
    public interface IEmailConfirmationService
    {
        Task<Guid> SendMessageAsync(string email, string ipAddress, string userAgent, CancellationToken ct);
    }
}