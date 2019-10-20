using System.ComponentModel.DataAnnotations;
using Ajupov.Identity.src.OAuth.Models.Tokens;
using Ajupov.Identity.src.OAuth.Models.Types;
using Ajupov.Utils.All.String;

namespace Ajupov.Identity.src.OAuth.Attributes.Validation
{
    public class RefreshTokenValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (validationContext.ObjectInstance is TokenRequest request &&
                request.grant_type == GrandType.RefreshToken &&
                value?.ToString().IsEmpty() == true)
            {
                return new ValidationResult("Invalid refresh token");
            }

            return ValidationResult.Success;
        }
    }
}