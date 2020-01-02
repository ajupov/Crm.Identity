using System.ComponentModel.DataAnnotations;
using Ajupov.Utils.All.String;
using Crm.Identity.OAuth.Models.Tokens;
using Crm.Identity.OAuth.Models.Types;

namespace Crm.Identity.OAuth.Attributes.Validation
{
    public class CodeValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(validationContext.ObjectInstance is TokenRequest request))
            {
                return new ValidationResult("Invalid access code");
            }

            if (request.grant_type != GrandType.AuthorizationCode)
            {
                return ValidationResult.Success;
            }

            if ((value?.ToString()).IsEmpty())
            {
                return new ValidationResult("Invalid access code");
            }

            return ValidationResult.Success;
        }
    }
}