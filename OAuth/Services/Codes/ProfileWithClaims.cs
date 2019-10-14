using System.Collections.Generic;
using Ajupov.Identity.OAuth.Services.Claims.Models;
using Ajupov.Identity.Profiles.Models;

namespace Ajupov.Identity.OAuth.Services.Codes
{
    public class ProfileWithClaims
    {
        public Profile Profile { get; set; }

        public List<Claim> Claims { get; set; }
    }
}