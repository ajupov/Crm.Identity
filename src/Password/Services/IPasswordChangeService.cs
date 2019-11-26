using System.Threading;
using System.Threading.Tasks;
using Crm.Identity.OAuth.Models.ChangePassword;

namespace Crm.Identity.Password.Services
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