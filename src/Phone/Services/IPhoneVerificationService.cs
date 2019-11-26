using System;
using System.Threading;
using System.Threading.Tasks;

namespace Crm.Identity.Phone.Services
{
    public interface IPhoneVerificationService
    {
        Task<bool> VerifyAsync(Guid tokenId, string code, CancellationToken ct);
    }
}