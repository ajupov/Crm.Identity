using System.Collections.Generic;
using Crm.Identity.Claims.Models;
using Crm.Identity.Codes.Models;
using Crm.Identity.Profiles.Models;

namespace Crm.Identity.Codes.Services
{
    public interface ICodesService
    {
        string Create(Profile profile, IEnumerable<Claim> claims);

        ProfileWithClaims Get(string code);
    }
}