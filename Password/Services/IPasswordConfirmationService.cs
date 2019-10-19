using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ajupov.Identity.Password.Services
{
    public interface IPasswordConfirmationService
    {
        Task<Guid> SendMessageAsync(string login, string ipAddress, string userAgent, CancellationToken ct);
    }
}