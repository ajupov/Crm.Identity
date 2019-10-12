using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.OAuth.Options;
using Microsoft.IdentityModel.Tokens;

namespace Ajupov.Identity.OAuth.Services.AccessTokens
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
                claims: claims,
                expires: now.AddMinutes(30),
                signingCredentials: credentials);

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(jwt));
        }
    }
}