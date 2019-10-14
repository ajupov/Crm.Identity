using System.ComponentModel.DataAnnotations;
using Ajupov.Identity.OAuth.Models.Tokens;
using Ajupov.Identity.OAuth.Models.Types;
using Ajupov.Utils.All.String;

namespace Ajupov.Identity.OAuth.Attributes.Validation
{
    public class PasswordValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (validationContext.ObjectInstance is TokenRequest request &&
                request.grant_type == GrandType.Password &&
                value?.ToString().IsEmpty() == true)
            {
                return new ValidationResult("Invalid username");
            }

            return ValidationResult.Success;
        }
    }
}