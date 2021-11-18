using System.Collections.Generic;

namespace Crm.Identity.Scopes
{
    public static class ScopesWithRoles
    {
        public static Dictionary<string, string[]> Value = new ()
        {
            { ScopeNames.Profile, new[] { "Profile" } },
            {
                ScopeNames.Api,
                new[] { "Account", "User", "Products", "Customers", "Orders", "Tasks", "Suppliers", "Stock" }
            }
        };
    }
}
