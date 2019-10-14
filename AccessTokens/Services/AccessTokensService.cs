using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.Claims.Models;
using Ajupov.Identity.OAuth.Options;
using Microsoft.IdentityModel.Tokens;
using SystemClaim = System.Security.Claims.Claim;

namespace Ajupov.Identity.AccessTokens.Services
{
    public class AccessTokensService : IAccessTokensService
    {
        public Task<string> CreateAsync(IEnumerable<Claim> claims, CancellationToken ct)
        {
            var now = DateTime.UtcNow;
            var credentials = new SigningCredentials(AuthOptions.GetKey(), SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                "Identity",
                notBefore: now,
                claims: claims.Select(x => new SystemClaim(x.Type, x.Value)),
                expires: now.AddMinutes(30),
                signingCredentials: credentials);

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(jwt));
        }
    }
}