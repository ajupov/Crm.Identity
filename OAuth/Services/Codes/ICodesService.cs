using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Ajupov.Identity.OAuth.Services.Codes
{
    public interface ICodesService
    {
        string Create(Identities.Models.Identity identity, List<Claim> claims);

        (Guid identityId, List<Claim> claims)? Get(string code);
    }
}