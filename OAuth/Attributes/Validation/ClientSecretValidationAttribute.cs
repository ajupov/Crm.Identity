using System.ComponentModel.DataAnnotations;
using Ajupov.Identity.OAuth.Models.Tokens;
using Ajupov.Identity.OAuth.Models.Types;
using Ajupov.Utils.All.String;

namespace Ajupov.Identity.OAuth.Attributes.Validation
{
    public class ClientSecretValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(validationContext.ObjectInstance is TokenRequest request))
            {
                return new ValidationResult("Invalid client secret");
            }

            if (request.grant_type != GrandType.AuthorizationCode)
            {
                return ValidationResult.Success;
            }

            if ((value?.ToString()).IsEmpty())
            {
                return new ValidationResult("Invalid client secret");
            }

            return ValidationResult.Success;
        }
    }
}