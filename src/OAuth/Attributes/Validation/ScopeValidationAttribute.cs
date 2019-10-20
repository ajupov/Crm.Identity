using System.ComponentModel.DataAnnotations;
using Ajupov.Utils.All.String;

namespace Ajupov.Identity.src.OAuth.Attributes.Validation
{
    public class ScopeValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if ((value?.ToString()).IsEmpty())
            {
                return new ValidationResult("Invalid scopes");
            }

            return ValidationResult.Success;
        }
    }
}