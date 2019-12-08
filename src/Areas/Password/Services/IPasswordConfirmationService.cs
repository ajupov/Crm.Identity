using System;
using System.Threading;
using System.Threading.Tasks;

namespace Crm.Identity.Areas.Password.Services
{
    public interface IPasswordConfirmationService
    {
        Task<Guid> SendMessageAsync(string key, string ipAddress, string userAgent, CancellationToken ct);
    }
}