using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Crm.Identity.Areas.OAuth.Options
{
    public class AuthOptions
    {
        public const string Issuer = "Identity";
        public const string Audience = "Identity";
        private const string Key = "VeryLargestSecretKey";

        public static SymmetricSecurityKey GetKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}