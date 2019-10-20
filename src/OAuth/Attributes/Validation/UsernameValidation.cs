using System.ComponentModel.DataAnnotations;
using Ajupov.Identity.src.OAuth.Models.Tokens;
using Ajupov.Identity.src.OAuth.Models.Types;
using Ajupov.Utils.All.String;

namespace Ajupov.Identity.src.OAuth.Attributes.Validation
{
    public class UsernameValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (validationContext.ObjectInstance is TokenRequest request &&
                request.grant_type == GrandType.Password &&
                value?.ToString().IsEmpty() == true)
            {
                return new ValidationResult("Invalid password");
            }

            return ValidationResult.Success;
        }
    }
}