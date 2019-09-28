using System.ComponentModel.DataAnnotations;
using Ajupov.Utils.All.String;

namespace Crm.Identity.OAuth.Attributes.Validation
{
    public class CodeValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if ((value?.ToString()).IsEmpty())
            {
                return new ValidationResult("Invalid access code");
            }

            return ValidationResult.Success;
        }
    }
}