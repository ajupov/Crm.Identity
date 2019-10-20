using System.Linq;
using Ajupov.Identity.src.Identities.Models;

namespace Ajupov.Identity.src.Identities.Extensions
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