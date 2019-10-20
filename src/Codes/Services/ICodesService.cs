using System.Collections.Generic;
using Ajupov.Identity.src.Claims.Models;
using Ajupov.Identity.src.Codes.Models;
using Ajupov.Identity.src.Profiles.Models;

namespace Ajupov.Identity.src.Codes.Services
{
    public interface ICodesService
    {
        string Create(Profile profile, IEnumerable<Claim> claims);

        ProfileWithClaims Get(string code);
    }
}