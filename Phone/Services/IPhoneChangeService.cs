using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.OAuth.Models.ChangePhone;

namespace Ajupov.Identity.Phone.Services
{
    public interface IPhoneChangeService
    {
        Task<PostChangePhoneResponse> ChangeAsync(
            string oldPhone,
            string newPhone,
            string password,
            string userAgent,
            string ipAddress,
            CancellationToken ct);
    }
}