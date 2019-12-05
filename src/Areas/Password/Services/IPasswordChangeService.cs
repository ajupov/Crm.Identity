using System.Threading;
using System.Threading.Tasks;
using Crm.Identity.Areas.OAuth.Models.ChangePassword;

namespace Crm.Identity.Areas.Password.Services
{
    public interface IPasswordChangeService
    {
        Task<PostChangePasswordResponse> ChangeAsync(
            string login,
            string oldPassword,
            string newPassword,
            CancellationToken ct);
    }
}