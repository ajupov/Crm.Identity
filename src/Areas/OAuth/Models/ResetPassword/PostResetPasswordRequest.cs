using Crm.Identity.Areas.OAuth.Attributes.Validation;

namespace Crm.Identity.Areas.OAuth.Models.ResetPassword
{
    public class PostResetPasswordRequest
    {
        [CountryValidation]
        public string Country { get; set; }

        public string Login { get; set; }
    }
}