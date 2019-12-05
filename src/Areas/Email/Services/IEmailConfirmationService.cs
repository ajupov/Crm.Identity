using System;
using System.Threading;
using System.Threading.Tasks;

namespace Crm.Identity.Areas.Email.Services
{
    public interface IEmailConfirmationService
    {
        Task<Guid> SendMessageAsync(string email, string ipAddress, string userAgent, CancellationToken ct);
    }
}