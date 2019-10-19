using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.OAuth.Models.ChangePassword;

namespace Ajupov.Identity.Password.Services
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