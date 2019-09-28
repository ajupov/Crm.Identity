using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Crm.Identity.OAuth.Options
{
    public class AuthOptions
    {
        public const string Issuer = "Identity";
        public const string Audience = "Identity";
        private const string Key = "SecretKey";

        public static SymmetricSecurityKey GetKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
        }
    }
}