using System.ComponentModel.DataAnnotations;
using Ajupov.Utils.All.String;

namespace Crm.Identity.OAuth.Attributes.Validation
{
    public class StateValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var stateString = value?.ToString();
            if (stateString.IsEmpty())
            {
                return new ValidationResult("Invalid state value");
            }

            return ValidationResult.Success;
        }
    }
}