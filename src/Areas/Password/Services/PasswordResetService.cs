using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Identity.Areas.Identities.Extensions;
using Crm.Identity.Areas.Identities.Models;
using Crm.Identity.Areas.Identities.Services;
using Crm.Identity.Areas.OAuth.Models.ResetPassword;
using Crm.Identity.Areas.Profiles.Services;

namespace Crm.Identity.Areas.Password.Services
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IProfilesService _profilesService;
        private readonly IIdentitiesService _identitiesService;
        private readonly IIdentityTokensService _identityTokensService;
        private readonly IPasswordConfirmationService _passwordConfirmationService;

        public PasswordResetService(
            IProfilesService profilesService,
            IIdentitiesService identitiesService,
            IIdentityTokensService identityTokensService,
            IPasswordConfirmationService passwordConfirmationService)
        {
            _profilesService = profilesService;
            _identitiesService = identitiesService;
            _identityTokensService = identityTokensService;
            _passwordConfirmationService = passwordConfirmationService;
        }

        public async Task<PostResetPasswordResponse> SendResetMessageAsync(
            string login,
            string userAgent,
            string ipAddress,
            CancellationToken ct)
        {
            var identityTypes = IdentityTypeExtensions.TypesWithPassword;
            var identity = await _identitiesService.GetVerifiedByKeyAndTypesAsync(login, identityTypes, ct);
            if (identity == null)
            {
                return new PostResetPasswordResponse(true);
            }

            var profile = await _profilesService.GetAsync(identity.ProfileId, ct);
            if (profile == null)
            {
                return new PostResetPasswordResponse(true);
            }

            await _passwordConfirmationService.SendMessageAsync(login, ipAddress, userAgent, ct);

            return new PostResetPasswordResponse(false);
        }

        public async Task<bool> IsTokenValidAsync(Guid tokenId, string code, CancellationToken ct)
        {
            var token = await _identityTokensService.GetAsync(tokenId, ct);

            return IsValidToken(token, code);
        }

        public async Task<SetNewPasswordResponse> SetNewPasswordAsync(
            Guid tokenId,
            string code,
            string newPassword,
            CancellationToken ct)
        {
            var token = await _identityTokensService.GetAsync(tokenId, ct);

            var isValidToken = IsValidToken(token, code);
            if (!isValidToken)
            {
                return new SetNewPasswordResponse(true);
            }

            var identity = await _identitiesService.GetAsync(token.IdentityId, ct);
            if (identity == null)
            {
                return new SetNewPasswordResponse(true);
            }

            await _identitiesService.ChangePasswordByProfileIdAsync(identity.ProfileId, newPassword, ct);

            return new SetNewPasswordResponse(false);
        }

        private static bool IsValidToken(IdentityToken token, string code)
        {
            return token != null &&
                   !token.UseDateTime.HasValue &&
                   token.ExpirationDateTime >= DateTime.UtcNow &&
                   token.Type == IdentityTokenType.PasswordReset &&
                   token.Value == code;
        }
    }
}