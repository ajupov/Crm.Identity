using System.Collections.Generic;
using Crm.Identity.Areas.Claims.Models;
using Crm.Identity.Areas.Codes.Models;
using Crm.Identity.Areas.Profiles.Models;

namespace Crm.Identity.Areas.Codes.Services
{
    public interface ICodesService
    {
        string Create(Profile profile, IEnumerable<Claim> claims);

        ProfileWithClaims Get(string code);
    }
}