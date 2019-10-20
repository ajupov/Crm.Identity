using System;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.src.Identities.Models;
using Ajupov.Identity.src.Identities.Services;

namespace Ajupov.Identity.src.Email.Services
{
    public class EmailVerificationService : IEmailVerificationService
    {
        private readonly IIdentitiesService _identitiesService;
        private readonly IIdentityTokensService _identityTokensService;

        public EmailVerificationService(
            IIdentitiesService identitiesService,
            IIdentityTokensService identityTokensService)
        {
            _identitiesService = identitiesService;
            _identityTokensService = identityTokensService;
        }

        public async Task<bool> VerifyAsync(Guid tokenId, string code, CancellationToken ct)
        {
            var token = await _identityTokensService.GetAsync(tokenId, ct);
            if (token == null ||
                token.UseDateTime.HasValue ||
                token.ExpirationDateTime < DateTime.UtcNow ||
                token.Type != IdentityTokenType.EmailValidation ||
                token.Value != code)
            {
                return false;
            }

            await _identityTokensService.SetIsUsedAsync(tokenId, ct);
            await _identitiesService.VerifyAsync(new[] {token.IdentityId}, ct);

            return true;
        }
    }
}