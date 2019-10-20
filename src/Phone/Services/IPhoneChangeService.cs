using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.src.OAuth.Models.ChangePhone;

namespace Ajupov.Identity.src.Phone.Services
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