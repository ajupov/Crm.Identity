using System.Linq;
using Crm.Identity.Identities.Models;

namespace Crm.Identity.Identities.Extensions
{
    public static class IdentityTypeExtensions
    {
        public static readonly IdentityType[] TypesWithPassword =
        {
            IdentityType.LoginAndPassword,
            IdentityType.EmailAndPassword,
            IdentityType.PhoneAndPassword
        };

        public static bool IsTypeWithPassword(this IdentityType type)
        {
            return TypesWithPassword.Contains(type);
        }
    }
}