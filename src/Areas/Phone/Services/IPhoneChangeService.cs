using System.Threading;
using System.Threading.Tasks;
using Crm.Identity.Areas.OAuth.Models.ChangePhone;

namespace Crm.Identity.Areas.Phone.Services
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