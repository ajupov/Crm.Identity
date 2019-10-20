using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ajupov.Identity.src.Phone.Services
{
    public interface IPhoneConfirmationService
    {
        Task<Guid> SendMessageAsync(string phone, string ipAddress, string userAgent, CancellationToken ct);
    }
}