using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ajupov.Identity.src.Email.Services
{
    public interface IEmailConfirmationService
    {
        Task<Guid> SendMessageAsync(string email, string ipAddress, string userAgent, CancellationToken ct);
    }
}