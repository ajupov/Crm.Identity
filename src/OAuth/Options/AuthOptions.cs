using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Ajupov.Identity.src.OAuth.Options
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