using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt.JwtGenerator;
using Crm.Identity.Claims.Models;
using SystemClaim = System.Security.Claims.Claim;

namespace Crm.Identity.AccessTokens.Services
{
    public class AccessTokensService : IAccessTokensService
    {
        private readonly IJwtGenerator _jwtGenerator;

        public AccessTokensService(IJwtGenerator jwtGenerator)
        {
            _jwtGenerator = jwtGenerator;
        }

        public Task<string> CreateAsync(IEnumerable<Claim> claimModels, CancellationToken ct)
        {
            var claims = claimModels.Select(x => new SystemClaim(x.Type, x.Value));
            var tokenString = _jwtGenerator.Generate(claims, TimeSpan.FromMinutes(30));

            return Task.FromResult(tokenString);
        }
    }
}