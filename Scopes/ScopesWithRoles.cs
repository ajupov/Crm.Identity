using System.Collections.Generic;

namespace Crm.Identity.Scopes
{
    public static class ScopesWithRoles
    {
        public static Dictionary<string, string[]> Value = new Dictionary<string, string[]>
        {
            {ScopeNames.Profile, new[] {"Profile"}},
            {ScopeNames.Api, new[] {"Products", "Leads", "Sales"}}
        };
    }
}