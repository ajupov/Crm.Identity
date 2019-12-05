using System.ComponentModel.DataAnnotations;
using Ajupov.Utils.All.String;
using Crm.Identity.Areas.OAuth.Models.Tokens;
using Crm.Identity.Areas.OAuth.Models.Types;

namespace Crm.Identity.Areas.OAuth.Attributes.Validation
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