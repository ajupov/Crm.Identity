using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ajupov.Identity.Email.Services
{
    public interface IEmailVerificationService
    {
        Task<bool> VerifyAsync(Guid tokenId, string code, CancellationToken ct);
    }
}