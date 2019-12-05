using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Identity.Areas.OAuth.Models.ResetPassword;

namespace Crm.Identity.Areas.Password.Services
{
    public interface IPasswordResetService
    {
        Task<PostResetPasswordResponse> SendResetMessageAsync(
            string login,
            string ipAddress,
            string userAgent,
            CancellationToken ct);

        Task<bool> IsTokenValidAsync(Guid tokenId, string code, CancellationToken ct);

        Task<SetNewPasswordResponse> SetNewPasswordAsync(
            Guid tokenId,
            string code,
            string newPassword,
            CancellationToken ct);
    }
}