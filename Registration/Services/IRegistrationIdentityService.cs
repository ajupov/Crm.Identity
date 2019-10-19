using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ajupov.Identity.Registration.Services
{
    public interface IRegistrationIdentityService
    {
        Task CreateLoginIdentityAsync(Guid profileId, string login, string passwordHash, CancellationToken ct);

        Task CreateEmailIdentityAsync(Guid profileId, string email, string passwordHash, CancellationToken ct);

        Task CreatePhoneIdentityAsync(Guid profileId, string phone, string passwordHash, CancellationToken ct);
    }
}