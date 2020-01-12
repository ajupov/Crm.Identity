using System;
using System.Collections.Generic;
using System.Linq;
using Ajupov.Infrastructure.All.Jwt.JwtGenerator;
using Ajupov.Infrastructure.All.Jwt.JwtReader;
using Crm.Identity.Claims.Models;
using SystemClaim = System.Security.Claims.Claim;

namespace Crm.Identity.AccessTokens.Services
{
    public class AccessTokensService : IAccessTokensService
    {
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IJwtReader _jwtReader;

        public AccessTokensService(IJwtGenerator jwtGenerator, IJwtReader jwtReader)
        {
            _jwtGenerator = jwtGenerator;
            _jwtReader = jwtReader;
        }

        public string Create(string audience, IEnumerable<Claim> claimModels)
        {
            var claims = claimModels.Select(x => new SystemClaim(x.Type, x.Value));

            return _jwtGenerator.Generate(AccessTokenDefaults.SigningKey, audience, claims,
                TimeSpan.FromSeconds(AccessTokenDefaults.ExpiresSeconds));
        }

        public List<Claim> Read(string tokenString)
        {
            return _jwtReader.ReadAccessToken(tokenString).Claims
                .Select(x => new Claim
                {
                    Type = x.Type,
                    Value = x.Value
                })
                .ToList();
        }
    }
}