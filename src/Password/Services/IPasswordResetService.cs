using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Identity.OAuth.Models.ResetPassword;

namespace Crm.Identity.Password.Services
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