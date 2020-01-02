using System;
using System.Threading;
using System.Threading.Tasks;

namespace Crm.Identity.Phone.Services
{
    public interface IPhoneConfirmationService
    {
        Task<Guid> SendMessageAsync(
            string country,
            string phone,
            string ipAddress,
            string userAgent,
            CancellationToken ct);
    }
}