using System.Collections.Generic;
using Ajupov.Identity.src.Claims.Models;
using Ajupov.Identity.src.Profiles.Models;

namespace Ajupov.Identity.src.Codes.Models
{
    public class ProfileWithClaims
    {
        public Profile Profile { get; set; }

        public List<Claim> Claims { get; set; }
    }
}