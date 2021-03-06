using System;
using System.Threading;
using System.Threading.Tasks;

namespace Crm.Identity.Email.Services
{
    public interface IEmailVerificationService
    {
        Task<bool> VerifyAsync(Guid tokenId, string code, CancellationToken ct);
    }
}