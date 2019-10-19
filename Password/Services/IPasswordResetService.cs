using System;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.OAuth.Models.ResetPassword;

namespace Ajupov.Identity.Password.Services
{
    public interface IPasswordResetService
    {
        Task<PostResetPasswordResponse> SendResetMessageAsync(
            string login,
            string userAgent,
            string ipAddress,
            CancellationToken ct);

        Task<bool> IsTokenValidAsync(Guid tokenId, string code, CancellationToken ct);

        Task<SetNewPasswordResponse> SetNewPasswordAsync(
            Guid tokenId,
            string code,
            string newPassword,
            CancellationToken ct);
    }
}