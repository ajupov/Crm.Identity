using Crm.Identity.OAuth.Attributes.Validation;

namespace Crm.Identity.OAuth.Models.ResetPassword
{
    public class PostResetPasswordRequest
    {
        [CountryValidation]
        public string Country { get; set; }

        public string Login { get; set; }
    }
}