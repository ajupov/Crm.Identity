using System.Collections.Generic;
using Crm.Identity.Claims.Models;
using Crm.Identity.Profiles.Models;

namespace Crm.Identity.Codes.Models
{
    public class ProfileWithClaims
    {
        public Profile Profile { get; set; }

        public List<Claim> Claims { get; set; }
    }
}