using System.Threading;
using System.Threading.Tasks;
using Crm.Identity.OAuth.Models.ChangePhone;

namespace Crm.Identity.Phone.Services
{
    public interface IPhoneChangeService
    {
        Task<PostChangePhoneResponse> ChangeAsync(
            string country,
            string oldPhone,
            string newPhone,
            string password,
            string ipAddress,
            string userAgent,
            CancellationToken ct);
    }
}