using System.Collections.Generic;
using Ajupov.Identity.Claims.Models;
using Ajupov.Identity.Profiles.Models;

namespace Ajupov.Identity.Codes.Models
{
    public class ProfileWithClaims
    {
        public Profile Profile { get; set; }

        public List<Claim> Claims { get; set; }
    }
}