using System.Collections.Generic;
using Ajupov.Identity.Claims.Models;
using Ajupov.Identity.Codes.Models;
using Ajupov.Identity.Profiles.Models;

namespace Ajupov.Identity.Codes.Services
{
    public interface ICodesService
    {
        string Create(Profile profile, IEnumerable<Claim> claims);

        ProfileWithClaims Get(string code);
    }
}