using System;
using System.Collections.Generic;
using System.Security.Claims;
using Ajupov.Infrastructure.All.HotStorage.HotStorage;
using Infrastructure.All.Generator;

namespace Ajupov.Identity.OAuth.Services.Codes
{
    public class CodesService : ICodesService
    {
        private readonly IHotStorage _hotStorage;

        public CodesService(IHotStorage hotStorage)
        {
            _hotStorage = hotStorage;
        }

        public string Create(Identities.Models.Identity identity, List<Claim> claims)
        {
            var code = Generator.GenerateAlphaNumericString(8);

            _hotStorage.SetValue(code, (identity.Id, claims), TimeSpan.FromMinutes(10));

            return code;
        }

        public (Guid identityId, List<Claim> claims)? Get(string code)
        {
            return _hotStorage.GetValue<(Guid identityId, List<Claim> claims)?>(code);
        }
    }
}