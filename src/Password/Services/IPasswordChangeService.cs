using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.src.OAuth.Models.ChangePassword;

namespace Ajupov.Identity.src.Password.Services
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