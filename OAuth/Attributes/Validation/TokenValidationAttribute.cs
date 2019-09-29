using System.ComponentModel.DataAnnotations;
using Ajupov.Utils.All.String;

namespace Ajupov.Identity.OAuth.Attributes.Validation
{
    public class TokenValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if ((value?.ToString()).IsEmpty())
            {
                return new ValidationResult("Invalid token");
            }

            return ValidationResult.Success;
        }
    }
}