using System;
using System.Collections.Generic;
using System.Linq;
using Ajupov.Infrastructure.All.Jwt.JwtGenerator;
using Ajupov.Infrastructure.All.Jwt.JwtReader;
using Ajupov.Infrastructure.All.Jwt.Settings;
using Crm.Identity.Claims.Models;
using Microsoft.Extensions.Options;
using SystemClaim = System.Security.Claims.Claim;

namespace Crm.Identity.AccessTokens.Services
{
    public class AccessTokensService : IAccessTokensService
    {
        private readonly JwtValidatorSettings _jwtValidatorSettings;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IJwtReader _jwtReader;

        public AccessTokensService(
            IOptions<JwtValidatorSettings> options,
            IJwtGenerator jwtGenerator,
            IJwtReader jwtReader)
        {
            _jwtValidatorSettings = options.Value;
            _jwtGenerator = jwtGenerator;
            _jwtReader = jwtReader;
        }

        public string Create(string audience, IEnumerable<Claim> claimModels)
        {
            var claims = claimModels.Select(x => new SystemClaim(x.Type, x.Value));

            return _jwtGenerator.Generate(_jwtValidatorSettings.SigningKey, audience, claims, TimeSpan.FromMinutes(60));
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