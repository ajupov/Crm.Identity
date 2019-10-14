﻿using System.Collections.Generic;
using Ajupov.Identity.OAuth.Services.Claims.Models;
using Ajupov.Identity.Profiles.Models;

namespace Ajupov.Identity.OAuth.Services.Codes
{
    public interface ICodesService
    {
        string Create(Profile profile, IEnumerable<Claim> claims);

        ProfileWithClaims? Get(string code);
    }
}