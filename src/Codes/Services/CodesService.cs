using System;
using System.Collections.Generic;
using System.Linq;
using Ajupov.Identity.src.Codes.Models;
using Ajupov.Identity.src.Profiles.Models;
using Ajupov.Infrastructure.All.HotStorage.HotStorage;
using Infrastructure.All.Generator;
using Claim = Ajupov.Identity.src.Claims.Models.Claim;

namespace Ajupov.Identity.src.Codes.Services
{
    public class CodesService : ICodesService
    {
        private readonly IHotStorage _hotStorage;

        public CodesService(IHotStorage hotStorage)
        {
            _hotStorage = hotStorage;
        }

        public string Create(Profile profile, IEnumerable<Claim> claims)
        {
            var code = Generator.GenerateAlphaNumericString(8);

            var profileWithClaims = new ProfileWithClaims
            {
                Profile = profile,
                Claims = claims
                    .Select(x => new Claim
                    {
                        Type = x.Type,
                        Value = x.Value
                    })
                    .ToList()
            };

            _hotStorage.SetValue(code, profileWithClaims, TimeSpan.FromMinutes(10));

            return code;
        }

        public ProfileWithClaims Get(string code)
        {
            return _hotStorage.GetValue<ProfileWithClaims>(code);
        }
    }
}