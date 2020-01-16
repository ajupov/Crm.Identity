using Microsoft.AspNetCore.Authorization;

namespace Crm.Identity.OAuth.Attributes.Roles
{
    public class RequireProfileRoleAttribute : AuthorizeAttribute
    {
        public RequireProfileRoleAttribute()
        {
            Roles = "Profile";
        }
    }
}