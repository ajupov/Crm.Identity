using System.Collections.Generic;
using Crm.Identity.Areas.Claims.Models;
using Crm.Identity.Areas.Profiles.Models;

namespace Crm.Identity.Areas.Codes.Models
{
    public class ProfileWithClaims
    {
        public Profile Profile { get; set; }

        public List<Claim> Claims { get; set; }
    }
}